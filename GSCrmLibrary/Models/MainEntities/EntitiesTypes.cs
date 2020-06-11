namespace GSCrmLibrary.Models.MainEntities
{
    public enum SearchSpecTypes
    {
        SearchSpecificationByParent,
        SearchSpecification,
        SearchSpecArgs
    }
    public enum ScreenItemTypes
    {
        AggregateView,
        AggregateCategory
    }
    public enum ActionType
    {
        None,
        NextRecords,
        ReloadView,
        UndoRecord,
        SelectTileItem,
        InitializeView,
        DeleteRecord,
        UndoUpdate,
        UpdateRecord,
        CancelQuery,
        OpenPickList,
        ApplyTable,
        ReloadScreen,
        PreviousRecords,
        Publish,
        SelectCrumb,
        SelectScreen,
        SelectScreenItem,
        Drilldown,
        ExecuteQuery,
        ShowPopup,
        InitializeScreen,
        ClosePopup,
        PickRecord,
        NewRecord,
        CopyRecord
    }
}
