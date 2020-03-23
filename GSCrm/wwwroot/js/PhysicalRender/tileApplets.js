var selectedRecords = new Map();
// После загрузки предыдущих записей в апплет
var PrevRecordsReady = new Event('PrevRecordsReady', [{ bubbles: true }]);
// После загрузки следующих записей в апплет
var NextRecordsReady = new Event('NextRecordsReady', [{ bubbles: true }]);
// До начала загрузки предыдущих записей в апплет
var PrevRecordsLoad = new Event('PrevRecordsLoad', [{ bubbles: true }]);
// До начала загрузки следующих записей в апплет
var NextRecordsLoad = new Event('NextRecordsLoad', [{ bubbles: true }]);