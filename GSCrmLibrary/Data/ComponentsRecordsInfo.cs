using System;
using System.Linq;
using System.Collections.Generic;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;
using GSCrmLibrary.Models.MainEntities;

namespace GSCrmLibrary.Data
{
    public static class ComponentsRecordsInfo
    {
        #region Свойства
        private static Dictionary<string, string> AllSelectedRecords { get; set; }
        private static Dictionary<string, List<string>> AllDisplayedRecords { get; set; }
        private static Dictionary<string, Dictionary<string, dynamic>> AllSearchSpecifications { get; set; }
        private static Dictionary<string, string> SelectedRecords { get; set; }
        private static Dictionary<string, List<string>> DisplayedRecords { get; set; }
        private static Dictionary<string, Dictionary<string, dynamic>> SearchSpecifications { get; set; }
        private static IEnumerable<ScreenItem> Crumbs { get; set; }
        #endregion

        #region Методы
        // Возвращает значение, укзаывающие, присутствует ли компонента в списке выбранных записей
        public static bool IsInitComponent(string busCompName)
        {
            if (SelectedRecords != null && busCompName != null && SelectedRecords.ContainsKey(busCompName))
                return true;
            else return false;
        }
        // Получение выбранных записей
        public static Dictionary<string, string> GetSelectedRecords()
        {
            if (SelectedRecords == null)
                return new Dictionary<string, string>();
            else return SelectedRecords;
        }
        // Получение выбранной записи
        public static string GetSelectedRecord(string componentName)
        {
            if (SelectedRecords == null)
                return null;
            else return SelectedRecords.GetValueOrDefault(componentName);
        }
        // Получение выбранных записей со всех компонент
        public static Dictionary<string, string> GetAllSelectedRecords() => AllSelectedRecords;
        // Установка выбранной записи
        public static void SetSelectedRecord(string componentName, string recordId)
        {
            if (AllSelectedRecords == null)
                AllSelectedRecords = new Dictionary<string, string>();
            if (SelectedRecords == null)
                SelectedRecords = new Dictionary<string, string>();
            if (!SelectedRecords.ContainsKey(componentName))
                SelectedRecords.Add(componentName, recordId);
            else SelectedRecords[componentName] = recordId;
            if (!AllSelectedRecords.ContainsKey(componentName))
                AllSelectedRecords.Add(componentName, recordId);
            else AllSelectedRecords[componentName] = recordId;
        }
        // Получение отоброжаемых записей
        public static List<string> GetDisplayedRecords(string componentName)
        {
            if (DisplayedRecords == null)
                return new List<string>();
            else return DisplayedRecords
                    .GetValueOrDefault(componentName)
                    ?.Where(val => val != null)?.ToList();
        }
        // Получение отоброжаемых записей со всех компонент
        public static List<string> GetAllDisplayedRecords(string componentName) => AllDisplayedRecords.GetValueOrDefault(componentName);
        // Установка отоброжаемых записей
        public static void SetDisplayedRecords(string componentName, List<string> recordsId)
        {
            if (AllDisplayedRecords == null)
                AllDisplayedRecords = new Dictionary<string, List<string>>();
            if (DisplayedRecords == null)
                DisplayedRecords = new Dictionary<string, List<string>>();
            List<string> records = recordsId?.Where(val => val != null)?.ToList();
            if (!DisplayedRecords.ContainsKey(componentName))
                DisplayedRecords.Add(componentName, records);
            else DisplayedRecords[componentName] = records;
            if (!AllDisplayedRecords.ContainsKey(componentName))
                AllDisplayedRecords.Add(componentName, records);
            else AllDisplayedRecords[componentName] = records;
        }
        // Получение спецификации для фильтрации
        public static dynamic GetSearchSpecification(string componentName, Enum searchSpecificationName)
        {
            if (SearchSpecifications == null)
            {
                SearchSpecifications = new Dictionary<string, Dictionary<string, dynamic>>();
                return null;
            }
            else if (SearchSpecifications.ContainsKey(componentName))
            {
                if (SearchSpecifications[componentName].ContainsKey(searchSpecificationName.ToString()))
                    return SearchSpecifications[componentName][searchSpecificationName.ToString()];
                else return null;
            }
            else return null;
        }
        // Установка спецификации для фильтрации
        public static void SetSearchSpecification(string componentName, Enum searchSpecificationName, dynamic searchSpecifiactionValue)
        {
            if (SearchSpecifications == null)
                SearchSpecifications = new Dictionary<string, Dictionary<string, dynamic>>();
            if (SearchSpecifications.ContainsKey(componentName))
            {
                if (SearchSpecifications[componentName].ContainsKey(searchSpecificationName.ToString()))
                    SearchSpecifications[componentName][searchSpecificationName.ToString()] = searchSpecifiactionValue;
                else SearchSpecifications[componentName].Add(searchSpecificationName.ToString(), searchSpecifiactionValue);
            }
            else
            {
                SearchSpecifications.Add(componentName, new Dictionary<string, dynamic>()
                {
                    { searchSpecificationName.ToString(), searchSpecifiactionValue }
                });
            }
            if (AllSearchSpecifications == null)
                AllSearchSpecifications = new Dictionary<string, Dictionary<string, dynamic>>();
            if (AllSearchSpecifications.ContainsKey(componentName))
            {

                if (AllSearchSpecifications[componentName].ContainsKey(searchSpecificationName.ToString()))
                    AllSearchSpecifications[componentName][searchSpecificationName.ToString()] = searchSpecifiactionValue;
                else AllSearchSpecifications[componentName].Add(searchSpecificationName.ToString(), searchSpecifiactionValue);
            }
            else
            {
                AllSearchSpecifications.Add(componentName, new Dictionary<string, dynamic>()
                {
                    { searchSpecificationName.ToString(), searchSpecifiactionValue }
                });
            }
        }
        // Возвращает текущие отображаемые записи для апплетов
        public static Dictionary<string, string> GetUIRecords(MainContext context, IViewInfo viewInfo)
        {
            Dictionary<string, string> UIRecords = new Dictionary<string, string>();
            SelectedRecords.ToList().ForEach(record =>
            {
                BusinessComponent busComp = context.BusinessComponents.FirstOrDefault(n => n.Name == record.Key);
                viewInfo.ViewApplets.Where(i => i.BusCompId == busComp.Id).ToList().ForEach(applet =>
                {
                    if (!UIRecords.ContainsKey(applet.Name))
                        UIRecords.Add(applet.Name, record.Value);
                });
            });
            return UIRecords;
        }
        // Очистка информации о старых отображаемых записях
        public static void ClearOldRecords(List<BusinessComponent> busComps)
        {
            List<string> recordsToRemove = new List<string>();

            // Названия новых отображаемых бизнес компонент
            List<string> busCompsNames = busComps.Select(n => n.Name).ToList();

            // Получение старых бизнес компонент, подлежащих удалению
            DisplayedRecords?.Keys.ToList().ForEach(bc =>
            {
                if (busCompsNames.IndexOf(bc) == -1)
                    recordsToRemove.Add(bc);
            });

            foreach (string name in recordsToRemove)
                DisplayedRecords.Remove(name);
        }
        // Получение хлебных крошек
        public static List<ScreenItem> GetCrumbs()
        {
            if (Crumbs == null)
                Crumbs = new List<ScreenItem>();
            return Crumbs.ToList();
        }
        // Добавление хлебной крошки
        public static void AppendCrumb(ScreenItem screenItem) => Crumbs = Crumbs.Append(screenItem);
        // Добавление хлебных крошек
        public static void AppendCrumbs(string actionName, Screen screen, ScreenItem oldScreenItem, ScreenItem newScreenItem)
        {
            if (Crumbs == null)
                Crumbs = new List<ScreenItem>();
            List<ScreenItem> crumbs = new List<ScreenItem>();

            switch (actionName)
            {
                case "SelectScreenItem":
                    ScreenItem oldItemCategory = screen.ScreenItems.FirstOrDefault(n => n.Id == oldScreenItem.ParentCategoryId);
                    ScreenItem newItemCategory = screen.ScreenItems.FirstOrDefault(n => n.Id == newScreenItem.ParentCategoryId);

                    // Если представления находятся в разных категориях, то просто добавляется ссылка на старое представление
                    if (newItemCategory != oldItemCategory)
                    {
                        oldScreenItem.Id = Guid.NewGuid();
                        Crumbs = Crumbs.Append(oldScreenItem);
                    }

                    // Иначе необходимо узнать, находятся ли представления в одной ветке графа
                    // Для этого вначале идет прохождение по предкам нового представления, с целью найти по пути старое
                    // Затем направление меняется и проход осуществляется по предкам старого представления, с целью найти новое
                    else
                    {
                        // Проход по предкам нового представления
                        ScreenItem screenItem = newScreenItem;
                        ScreenItem parentItem = screen.ScreenItems.FirstOrDefault(n => n.Id == screenItem.ParentItemId);
                        while (parentItem != null && parentItem != oldScreenItem)
                        {
                            crumbs.Add(parentItem);
                            parentItem = screen.ScreenItems.FirstOrDefault(n => n.Id == parentItem.ParentItemId);
                        }

                        if (parentItem == null)
                        {
                            // Проход по предкам старого представления
                            crumbs = new List<ScreenItem>();
                            screenItem = oldScreenItem;
                            crumbs.Add(oldScreenItem);
                            parentItem = screen.ScreenItems.FirstOrDefault(n => n.Id == screenItem.ParentItemId);
                            while (parentItem != null && parentItem != newScreenItem)
                            {
                                crumbs.Add(parentItem);
                                parentItem = screen.ScreenItems.FirstOrDefault(n => n.Id == parentItem.ParentItemId);
                            }

                            // В случае, если представления находятся в разных ветках, то просто добавляется ссылка на старое представление
                            if (parentItem == null)
                            {
                                crumbs = new List<ScreenItem>();
                                oldScreenItem.Id = Guid.NewGuid();
                                Crumbs = Crumbs.Append(oldScreenItem);
                            }

                            // В случае, если представления находятся в одной ветке, то добавляются ссылки на все представления между старым и новым
                            else crumbs.ForEach(crumb =>
                            {
                                crumb.Id = Guid.NewGuid();
                                Crumbs = Crumbs.Append(crumb);
                            });
                        }

                        // Иначе добавляются ссылки на все представления между новым и старым
                        else
                        {
                            crumbs.Add(oldScreenItem);
                            crumbs.AsEnumerable().Reverse().ToList().ForEach(crumb =>
                            {
                                crumb.Id = Guid.NewGuid();
                                Crumbs = Crumbs.Append(crumb);
                            });
                        }
                    }
                    break;
            }
        }
        // Удаление хлебных крошек
        public static void RemoveCrumbs(Guid crumbId)
            => Crumbs = Crumbs.Reverse().SkipWhile(i => i.Id != crumbId).Skip(1).Reverse();
        // Восстановление контекста
        public static void RestoreContext(List<View> views)
        {
            views.ForEach(view =>
            {
                view.BusObject.BusObjectComponents.ForEach(component =>
                {
                    SetSelectedRecord(component.BusComp.Name, AllSelectedRecords.GetValueOrDefault(component.Name));
                    SetDisplayedRecords(component.BusComp.Name, AllDisplayedRecords.GetValueOrDefault(component.Name));
                    if (AllSearchSpecifications != null)
                    {
                        Dictionary<string, dynamic> searchSpecifications = AllSearchSpecifications.GetValueOrDefault(component.Name);
                        if (searchSpecifications != null)
                        {
                            if (searchSpecifications.ContainsKey("SearchSpecArgs"))
                                SetSearchSpecification(component.Name, SearchSpecTypes.SearchSpecArgs, searchSpecifications["SearchSpecArgs"]);
                            if (searchSpecifications.ContainsKey("SearchSpecification"))
                                SetSearchSpecification(component.Name, SearchSpecTypes.SearchSpecification, searchSpecifications["SearchSpecification"]);
                            if (searchSpecifications.ContainsKey("SearchSpecificationByParent"))
                                SetSearchSpecification(component.Name, SearchSpecTypes.SearchSpecificationByParent, searchSpecifications["SearchSpecificationByParent"]);
                        }
                    }
                });
            });
        }
        // Очистка информации
        public static void Dispose()
        {
            SelectedRecords = null;
            DisplayedRecords = null;
            SearchSpecifications = null;
        }
        #endregion
    }
}
