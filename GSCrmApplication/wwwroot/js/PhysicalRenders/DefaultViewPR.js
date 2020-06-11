export default class DefaultViewPR {
    RenderView(view) {
        // Представление и его Id
        let $view = $('<div class="gs-view" data-type="view" data-name="' + view['Name'] + '"></div>');
        let viewId = "GSV_0";
        let counter = 0;

        // До тех пор, пока элемент с таким id присутствует на странице, прибавляю 1, чтобы Id был уникальным
        while ($('#' + viewId).length > 0) {
            viewId = viewId.split(counter).join((counter + 1));
            counter++;
        }

        $view.attr('id', viewId);
        return $view;
    }
}