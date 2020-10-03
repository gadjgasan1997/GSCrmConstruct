using GSCrm.Data;
using GSCrm.Data.ApplicationInfo;
using GSCrm.DataTransformers;
using GSCrm.Helpers;
using GSCrm.Localization;
using GSCrm.Models;
using GSCrm.Models.ViewModels;
using GSCrm.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static GSCrm.CommonConsts;
using static GSCrm.Utils.CollectionsUtils;

namespace GSCrm.Repository
{
    public class EmployeeRepository : GenericRepository<Employee, EmployeeViewModel, EmployeeValidator, EmployeeTransformer>
    {
        private User user;
        private readonly UserManager<User> userManager;
        private readonly User currentUser;
        private Organization currentOrganization;
        public static EmployeeViewModel CurrentEmployee { get; set; }
        public EmployeeRepository(ApplicationDbContext context, ResManager resManager) : base(context, resManager) { }
        public EmployeeRepository(ApplicationDbContext context, IViewsInfo viewsInfo, ResManager resManager, User currentUser = null, UserManager<User> userManager = null)
            : base(context, viewsInfo, resManager, new EmployeeValidator(userManager, context, resManager), new EmployeeTransformer(context, resManager))
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
        }

        #region Override Methods
        public override bool TryCreatePrepare(EmployeeViewModel employeeViewModel, ModelStateDictionary modelState)
        {
            if (!base.TryCreatePrepare(employeeViewModel, modelState)) return false;
            SetUpCurrentOrganization(employeeViewModel);
            if (!TryPrepareUserAccount(employeeViewModel, modelState)) return false;
            SetUpUserOrganization(user);
            employeeViewModel.UserId = user.Id;

            // Проставление Id основной организации новому сотруднику
            if (user.PrimaryOrganizationId == Guid.Empty)
                user.PrimaryOrganizationId = currentUser.PrimaryOrganizationId;
            context.Users.Update(user);
            return true;
        }

        public override void FailureUpdateHandler(EmployeeViewModel employeeViewModel, Action<EmployeeViewModel> handler = null)
        {
            if (TryGetItemById(employeeViewModel.Id, out Employee employee))
            {
                employeeViewModel = transformer.DataToViewModel(employee);
                employeeViewModel = transformer.UpdateViewModelFromCash(employeeViewModel);
                AttachContacts(employeeViewModel);
                AttachPositions(employeeViewModel);
            }
        }

        public override bool TryDeletePrepare(Guid id, Employee employee, ModelStateDictionary modelState)
        {
            if (!base.TryDeletePrepare(id, employee, modelState)) return false;
            RemoveUserOrganization(employee);
            CheckAccountsForLock(employee);
            return true;
        }
        #endregion

        #region Searching
        /// <summary>
        /// Метод устанавливает значения для поиска по должностям
        /// </summary>
        /// <param name="empViewModelCash"></param>
        /// <returns></returns>
        public void SearchPosition(EmployeeViewModel employeeViewModel)
        {
            viewsInfo.Reset(EMP_POSITIONS);
            EmployeeViewModel empViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_POSITIONS);
            empViewModelCash.DivisionId = employeeViewModel.DivisionId;
            empViewModelCash.SearchPosName = employeeViewModel.SearchPosName?.ToLower().TrimStartAndEnd();
            empViewModelCash.SearchParentPosName = employeeViewModel.SearchParentPosName?.ToLower().TrimStartAndEnd();
            ModelCash<EmployeeViewModel>.SetViewModel(EMP_POSITIONS, empViewModelCash);
        }

        /// <summary>
        /// Метод очищает поиск по должностям
        /// </summary>
        public void ClearPositionSearch()
        {
            EmployeeViewModel empViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_POSITIONS);
            empViewModelCash.SearchPosName = default;
            empViewModelCash.SearchParentPosName = default;
            ModelCash<EmployeeViewModel>.SetViewModel(EMP_POSITIONS, empViewModelCash);
        }

        /// <summary>
        /// Метод устанавливает значения для поиска по должностям
        /// </summary>
        /// <param name="empViewModelCash"></param>
        /// <returns></returns>
        public void SearchContact(EmployeeViewModel employeeViewModel)
        {
            viewsInfo.Reset(EMP_CONTACTS);
            EmployeeViewModel empViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_CONTACTS);
            empViewModelCash.DivisionId = employeeViewModel.DivisionId;
            empViewModelCash.SearchContactType = employeeViewModel.SearchContactType;
            empViewModelCash.SearchContactPhone = employeeViewModel.SearchContactPhone?.ToLower().TrimStartAndEnd();
            empViewModelCash.SearchContactEmail = employeeViewModel.SearchContactEmail?.ToLower().TrimStartAndEnd();
            ModelCash<EmployeeViewModel>.SetViewModel(EMP_CONTACTS, empViewModelCash);
        }

        /// <summary>
        /// Метод очищает поиск по должностям
        /// </summary>
        public void ClearContactSearch()
        {
            EmployeeViewModel empViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_CONTACTS);
            empViewModelCash.SearchContactType = string.Empty;
            empViewModelCash.SearchContactPhone = default;
            empViewModelCash.SearchContactEmail = default;
            ModelCash<EmployeeViewModel>.SetViewModel(EMP_CONTACTS, empViewModelCash);
        }

        /// <summary>
        /// Метод устанавливает значения для поиска по подчиненным
        /// </summary>
        /// <param name="empViewModelCash"></param>
        /// <returns></returns>
        public void SearchSubordinate(EmployeeViewModel employeeViewModel)
        {
            viewsInfo.Reset(EMP_SUBS);
            EmployeeViewModel empViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_SUBS);
            empViewModelCash.SearchSubordinateFullName = employeeViewModel.SearchSubordinateFullName?.ToLower().TrimStartAndEnd();
            ModelCash<EmployeeViewModel>.SetViewModel(EMP_SUBS, empViewModelCash);
        }

        /// <summary>
        /// Метод очищает поиск по подчиненным
        /// </summary>
        public void ClearSubordinateSearch()
        {
            EmployeeViewModel empViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_SUBS);
            empViewModelCash.SearchSubordinateFullName = default;
            ModelCash<EmployeeViewModel>.SetViewModel(EMP_SUBS, empViewModelCash);
        }
        #endregion

        #region Attaching Positions
        /// <summary>
        /// Добавляет должности к сотруднику, преобразую данные в отображение для выбранной страницы
        /// </summary>
        /// <returns></returns>
        public void AttachPositions(EmployeeViewModel employeeViewModel)
        {
            employeeViewModel.EmployeePositionViewModels = employeeViewModel.GetPositions(context)
                .TransformToViewModels<EmployeePosition, EmployeePositionViewModel, EmployeePositionTransformer>(
                    transformer: new EmployeePositionTransformer(context, resManager),
                    limitingFunc: GetLimitedPositionsList);
        }

        private List<EmployeePosition> GetLimitedPositionsList(List<EmployeePosition> positions)
        {
            EmployeeViewModel employeeViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_POSITIONS);
            List<EmployeePosition> limitedPositions = positions;
            LimitPosByName(employeeViewModelCash, ref limitedPositions);
            LimitPosByParent(employeeViewModelCash, ref limitedPositions);
            LimitListByPageNumber(EMP_POSITIONS, ref limitedPositions);
            return limitedPositions;
        }

        /// <summary>
        /// Ограничение списка подразделений по названию
        /// </summary>
        /// <param name="employeeViewModelCash"></param>
        /// <param name="positionsToLimit"></param>
        private void LimitPosByName(EmployeeViewModel employeeViewModelCash, ref List<EmployeePosition> positionsToLimit)
        {
            if (!string.IsNullOrEmpty(employeeViewModelCash.SearchPosName))
            {
                TransformCollection(
                    collectionToLimit: ref positionsToLimit,
                    limitingCollection: employeeViewModelCash.GetDivision(context).Positions,
                    limitCondition: n => n.Name.ToLower().Contains(employeeViewModelCash.SearchPosName),
                    selectCondition: i => i.Id,
                    removeCondition: (positionIdList, employeePosition) => employeePosition.PositionId == null || !positionIdList.Contains((Guid)employeePosition.PositionId));
            }
        }

        /// <summary>
        /// Ограничение списка подразделений по названию родительского
        /// </summary>
        /// <param name="employeeViewModelCash"></param>
        /// <param name="positionsToLimit"></param>
        private void LimitPosByParent(EmployeeViewModel employeeViewModelCash, ref List<EmployeePosition> positionsToLimit)
            => positionsToLimit = positionsToLimit.LimitByParent(context, employeeViewModelCash, employeeViewModelCash.SearchParentPosName);
        #endregion

        #region Attaching Contacts
        /// <summary>
        /// Добавляет контакты к сотруднику, преобразую данные в отображение для выбранной страницы
        /// </summary>
        /// <returns></returns>
        public void AttachContacts(EmployeeViewModel employeeViewModel)
        {
            employeeViewModel.EmployeeContactViewModels = employeeViewModel.GetContacts(context)
                .TransformToViewModels<EmployeeContact, EmployeeContactViewModel, EmployeeContactTransformer>(
                    transformer: new EmployeeContactTransformer(context, resManager),
                    limitingFunc: GetLimitedContactsList);
        }

        private List<EmployeeContact> GetLimitedContactsList(List<EmployeeContact> contacts)
        {
            EmployeeViewModel employeeViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_CONTACTS);
            List<EmployeeContact> limitedContacts = contacts;
            LimitContactsByType(employeeViewModelCash, ref limitedContacts);
            LimitContactsByPhone(employeeViewModelCash, ref limitedContacts);
            LimitContactsByEmail(employeeViewModelCash, ref limitedContacts);
            LimitListByPageNumber(EMP_CONTACTS, ref limitedContacts);
            return limitedContacts;
        }

        /// <summary>
        /// Ограничение списка контактов по типу
        /// </summary>
        /// <param name="employeeViewModelCash"></param>
        /// <param name="contactsToLimit"></param>
        private void LimitContactsByType(EmployeeViewModel employeeViewModelCash, ref List<EmployeeContact> contactsToLimit)
        {
            if (!string.IsNullOrEmpty(employeeViewModelCash.SearchContactType))
                contactsToLimit = contactsToLimit.Where(t => t.ContactType.ToString() == employeeViewModelCash.SearchContactType).ToList();
        }

        /// <summary>
        /// Ограничение списка контактов по телефону
        /// </summary>
        /// <param name="employeeViewModelCash"></param>
        /// <param name="contactsToLimit"></param>
        private void LimitContactsByPhone(EmployeeViewModel employeeViewModelCash, ref List<EmployeeContact> contactsToLimit)
        {
            if (!string.IsNullOrEmpty(employeeViewModelCash.SearchContactPhone))
                contactsToLimit = contactsToLimit.Where(p => p.PhoneNumber.ToLower().Contains(employeeViewModelCash.SearchContactPhone)).ToList();
        }

        /// <summary>
        /// Ограничение списка контактов по почте
        /// </summary>
        /// <param name="employeeViewModelCash"></param>
        /// <param name="contactsToLimit"></param>
        private void LimitContactsByEmail(EmployeeViewModel employeeViewModelCash, ref List<EmployeeContact> contactsToLimit)
        {
            if (!string.IsNullOrEmpty(employeeViewModelCash.SearchContactEmail))
                contactsToLimit = contactsToLimit.Where(p => p.Email.ToLower().Contains(employeeViewModelCash.SearchContactEmail)).ToList();
        }
        #endregion

        #region Attaching Subordinates
        /// <summary>
        /// Добавляет подчиненных к сотруднику, преобразую данные в отображение для выбранной страницы
        /// </summary>
        /// <returns></returns>
        public void AttachSubordinates(EmployeeViewModel employeeViewModel)
        {
            employeeViewModel.SubordinatesViewModels = employeeViewModel.GetSubordinates(context)
                .TransformToViewModels<Employee, EmployeeViewModel, EmployeeTransformer>(
                    transformer: new EmployeeTransformer(context, resManager),
                    limitingFunc: GetLimitedSubordinatesList);
        }

        private List<Employee> GetLimitedSubordinatesList(List<Employee> employees)
        {
            EmployeeViewModel employeeViewModelCash = ModelCash<EmployeeViewModel>.GetViewModel(EMP_SUBS);
            List<Employee> limitedSubordinates = employees;
            LimitSubordinatesByFullName(employeeViewModelCash, ref limitedSubordinates);
            LimitListByPageNumber(EMP_SUBS, ref limitedSubordinates);
            return limitedSubordinates;
        }

        /// <summary>
        /// Метод ограничивает список подчиненных по полному имени
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <param name="employeesToLimit"></param>
        private void LimitSubordinatesByFullName(EmployeeViewModel employeeViewModel, ref List<Employee> employeesToLimit)
        {
            if (!string.IsNullOrEmpty(employeeViewModel.SearchSubordinateFullName))
                employeesToLimit = employeesToLimit.Where(emp => emp.GetFullName().ToLower().Contains(employeeViewModel.SearchSubordinateFullName)).ToList();
        }
        #endregion

        #region Other Methods
        /// <summary>
        /// Удаление записи из таблицы "UserOrganizations"
        /// </summary>
        /// <param name="employeeToDelete"></param>
        private void RemoveUserOrganization(Employee employeeToDelete)
        {
            Organization organization = employeeToDelete.GetOrganization(context);
            Func<UserOrganization, bool> predicate = i => i.UserId == employeeToDelete.UserId.ToString() && i.OrganizationId == organization.Id;
            UserOrganization userOrganization = context.UserOrganizations.FirstOrDefault(predicate);
            context.Entry(userOrganization).State = EntityState.Deleted;
            context.UserOrganizations.Remove(userOrganization);
        }

        /// <summary>
        /// Метод проверяет клиентов, у которых удаляемы пользователь является основным клиентским менеджером
        /// Если в команде найденного клиента присутствуют другие менеджеры,, метод делает основным случайного
        /// Иначе блокирует клиента
        /// </summary>
        /// <param name="employee"></param>
        private void CheckAccountsForLock(Employee employee)
        {
            // Список всех основных клиентских менеджеров, где сотрудником является поданный "employee"
            Func<AccountManager, bool> predicate = accNam => accNam.ManagerId == employee.Id && accNam.Account.PrimaryManagerId == accNam.Id;
            List<AccountManager> accountManagers = context.AccountManagers.Include(acc => acc.Account).Where(predicate).ToList();
            accountManagers.Select(acc => acc.Account).ToList().ForEach(account =>
            {
                List<AccountManager> allAccManagers = context.AccountManagers.Where(acc => acc.AccountId == account.Id).ToList();
                if (allAccManagers.Count <= 1)
                {
                    account.AccountStatus = AccountStatus.Lock;
                    account.PrimaryManagerId = Guid.Empty;
                }
                else account.PrimaryManagerId = allAccManagers.FirstOrDefault(i => i.ManagerId != employee.Id).Id;
                context.Accounts.Update(account);
            });
        }

        /// <summary>
        /// Метод выполняет попытку смены подразделения и, в случае неуспеха, записывает в модель состояния ошибки
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool TryChangeDivision(EmployeeViewModel employeeViewModel, ModelStateDictionary modelState)
        {
            if (TryChangeDivisionValidate(employeeViewModel, modelState))
            {
                SetUpCurrentOrganization(employeeViewModel);
                Division newDivision = GetNewDivision(employeeViewModel);
                Position newPrimaryPosition = GetNewPrimaryPosition(employeeViewModel, newDivision);
                Employee employee = context.Employees.Include(pos => pos.EmployeePositions).FirstOrDefault(i => i.Id == employeeViewModel.Id);
                SetNewDivision(employee, newDivision);
                RemoveOldEmployeePositions(employee);
                AddEmployeePosition(employee, newPrimaryPosition);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Метод пытает произвести разблокировку сотрудника, возникшую в результате отсутствия у него должности и/или подразделения
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public bool TryUnlock(ref EmployeeViewModel employeeViewModel, ModelStateDictionary modelState)
        {
            // Получение сотрудника из бд
            Guid employeeId = employeeViewModel.Id;
            Employee employee = context.Employees
                .Include(empPos => empPos.EmployeePositions)
                .FirstOrDefault(i => i.Id == employeeId);

            /* Если валидация проходит успешно, происходят действия по разблокировке и
            данные из бд преобразуются в данные для отображения с прикреплением контактов и должностей */
            if (TryUnlockValidate(employeeViewModel, modelState))
            {
                UnlockEmployeeSetDivision(employeeViewModel, employee);
                UnlockEmployeeSetPosition(employeeViewModel, employee);
                employee.Unlock();
                context.Update(employee);
                context.SaveChanges();

                employeeViewModel = transformer.DataToViewModel(employee);
                AttachPositions(employeeViewModel);
                AttachContacts(employeeViewModel);
                return true;
            }

            // Иначе данные из бд преобразуются в данные для отображения без прикрепления контактов и должностей
            else
            {
                employeeViewModel = transformer.DataToViewModel(employee);
                return false;
            }
        }

        /// <summary>
        /// Метод устанавливает новое подразделение при разблокировке сотруднкиа
        /// </summary>
        /// <param name="employeeViewModel"></param>
        private void UnlockEmployeeSetDivision(EmployeeViewModel employeeViewModel, Employee employee)
        {
            Division employeeDivision = employee.GetDivision(context);
            if (employeeDivision.Name != employeeViewModel.DivisionName)
            {
                List<Division> divisions = context.GetOrgDivisions(employeeViewModel.OrganizationId);
                Division newDivision = divisions.FirstOrDefault(n => n.Name == employeeViewModel.DivisionName);
                SetNewDivision(employee, newDivision);
            }
        }

        /// <summary>
        /// Метод устанавливает новую должность при разблокировке сотрудника
        /// </summary>
        /// <param name="employeeViewModel"></param>
        private void UnlockEmployeeSetPosition(EmployeeViewModel employeeViewModel, Employee employee)
        {
            Division employeeDivision = employee.GetDivision(context);
            Position newPosition = employeeDivision.Positions.FirstOrDefault(n => n.Name == employeeViewModel.PrimaryPositionName);
            if (!employee.EmployeePositions.ContainsPosition(newPosition))
                AddEmployeePosition(employee, newPosition);
            SetPrimaryPosition(employee, newPosition);
        }

        /// <summary>
        /// Устанаваливает текущую организацию
        /// </summary>
        /// <param name="employeeViewModel"></param>
        private void SetUpCurrentOrganization(EmployeeViewModel employeeViewModel) => currentOrganization = employeeViewModel.GetOrganization(context);

        /// <summary>
        /// Метод проверяет существует ли пользователь с таким именем в организации, и если нет, создает его
        /// </summary>
        /// <param name="employeeViewModel"></param>
        private bool TryPrepareUserAccount(EmployeeViewModel employeeViewModel, ModelStateDictionary modelState)
        {
            if (employeeViewModel.UserAccountExists)
                user = context.Users.FirstOrDefault(n => n.UserName == employeeViewModel.UserName);
            else
            {
                user = new User()
                {
                    UserName = employeeViewModel.UserName,
                    Email = employeeViewModel.Email,
                    EmailConfirmed = true
                };
                IdentityResult identityResult = userManager.CreateAsync(user, employeeViewModel.Password).Result;
                if (!identityResult.Succeeded)
                {
                    foreach (IdentityError identityError in identityResult.Errors)
                        modelState.AddModelError(identityError.Code, identityError.Description);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Метод добавляет организацию в список организаций пользователя
        /// </summary>
        /// <param name="employeeViewModel"></param>
        private void SetUpUserOrganization(User user)
        {
            user.UserOrganizations.Add(new UserOrganization()
            {
                Organization = currentOrganization,
                OrganizationId = currentOrganization.Id,
                User = user,
                UserId = user.Id
            });
        }

        /// <summary>
        /// Метод проверяет модель при изменении подразделения на сотруднике
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private bool TryChangeDivisionValidate(EmployeeViewModel employeeViewModel, ModelStateDictionary modelState)
        {
            Dictionary<string, string> errors = validator.ChangeDivisionCheck(employeeViewModel);
            if (errors.Any())
            {
                foreach (KeyValuePair<string, string> error in errors)
                    modelState.AddModelError(error.Key, error.Value);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод проверяет модель при разблокировке сотрудника
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private bool TryUnlockValidate(EmployeeViewModel employeeViewModel, ModelStateDictionary modelState)
        {
            Dictionary<string, string> errors = validator.UnlockValidateCheck(employeeViewModel);
            if (errors.Any())
            {
                foreach (KeyValuePair<string, string> error in errors)
                    modelState.AddModelError(error.Key, error.Value);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Метод возвращает новое подразделение при смене подразделения на сотруднике
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <returns></returns>
        private Division GetNewDivision(EmployeeViewModel employeeViewModel)
        {
            string newDivisionName = employeeViewModel.DivisionName;
            return currentOrganization.Divisions.FirstOrDefault(n => n.Name == newDivisionName);
        }

        /// <summary>
        /// Метод возвращает новую должность при смене подразделения на сотруднике
        /// </summary>
        /// <param name="employeeViewModel"></param>
        /// <param name="newDivision">Новое подразделение</param>
        /// <returns></returns>
        private Position GetNewPrimaryPosition(EmployeeViewModel employeeViewModel, Division newDivision)
        {
            string primaryPositionName = employeeViewModel.PrimaryPositionName;
            return newDivision.Positions.FirstOrDefault(n => n.Name == primaryPositionName);
        }

        /// <summary>
        /// Метод устанавливает новое подразделение у сотрудника
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="newDivision"></param>
        private void SetNewDivision(Employee employee, Division newDivision) => employee.DivisionId = newDivision.Id;

        /// <summary>
        /// Метод создает и добавляет новую должность в список должностей сотрудника
        /// и устанавливает пришедшую на вход должность как основную
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="newPrimaryPosition"></param>
        private void AddEmployeePosition(Employee employee, Position newPrimaryPosition)
        {
            EmployeePosition employeePosition = new EmployeePosition()
            {
                Id = Guid.NewGuid(),
                Employee = employee,
                EmployeeId = employee.Id,
                Position = newPrimaryPosition,
                PositionId = newPrimaryPosition.Id
            };
            SetPrimaryPosition(employee, newPrimaryPosition);
            context.EmployeePositions.Add(employeePosition);
        }

        /// <summary>
        /// Метод устанавливает для сотрудника основную должность
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="primaryPosition"></param>
        private void SetPrimaryPosition(Employee employee, Position primaryPosition) => employee.PrimaryPositionId = primaryPosition.Id;

        /// <summary>
        /// Удаление из контекста всех должностей сотрудника
        /// </summary>
        /// <param name="employee"></param>
        private void RemoveOldEmployeePositions(Employee employee)
        {
            employee.EmployeePositions.ForEach(employeePosition =>
            {
                context.EmployeePositions.Remove(employeePosition);
            });
        }
        #endregion
    }
}
