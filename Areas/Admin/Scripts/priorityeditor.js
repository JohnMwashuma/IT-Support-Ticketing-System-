$(document).ready(function () {

    var $priorityEditor = $(".ticket-priority-editor");

    $priorityEditor
            .find(".priority-select")
            .on("click", "> li > a", function (e) {
                e.preventDefault();

                var $this = $(this);
                var $priorityParent = $this.closest("li");
                $priorityParent.toggleClass("selected");

                var selected = $priorityParent.hasClass("selected");
                $priorityParent.find(".selected-input").val(selected);
            });

    var $addPriorityButton = $priorityEditor.find(".add-priority-button");
    var $newPriorityName = $priorityEditor.find(".new-priority-name");

    function addPriority(name) {
        var newIndex = $priorityEditor.find(".priority-select >li").size() - 1;

        $priorityEditor
            .find(".priority-select > li.template")
            .clone()
            .removeClass("template")
            .addClass("selected")
            .find(".name").text(name).end()
            .find(".name-input").val(name).attr("name", "Priorities[" + newIndex + "].Name").end()
            .find(".selected-input").attr("name", "Priorities[" + newIndex + "].IsChecked").val(true).end()
            .appendTo($priorityEditor.find(".priority-select"));

        $newPriorityName.val("");
        $addPriorityButton.pop("disabled", true);
    }

    $addPriorityButton.click(function (e) {
        e.preventDefault();
        addPriority($addPriorityButton.val());
    });

    $newPriorityName
        .keyup(function () {
            if ($newPriorityName.val().trim().length > 0)
                $addPriorityButton.prop("disabled", false);
            else
                $addPriorityButton.prop("disabled", true);

        })
        .keydown(function (e) {
            if (e.which !== 13)
                return;

            e.preventDefault();
            addPriority($newPriorityName.val());

        });



});