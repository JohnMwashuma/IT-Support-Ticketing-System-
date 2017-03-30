$(document).ready(function () {

    var $commentEditor = $(".ticket-comment-editor");

    $commentEditor
            .find(".comment-select")
            .on("click", "> li > a", function (e) {
                e.preventDefault();

                var $this = $(this);
                var $commentParent = $this.closest("li");
                $commentParent.toggleClass("selected");

                var selected = $commentParent.hasClass("selected");
                $commentParent.find(".selected-input").val(selected);
            });

    var $addCommentButton = $commentEditor.find(".add-comment-button");
    var $newCommentName = $commentEditor.find(".new-comment-name");

    function addComment(name) {
        var newIndex = $commentEditor.find(".comment-select >li").size() - 1;

        $commentEditor
            .find(".comment-select > li.template")
            .clone()
            .removeClass("template")
            .addClass("selected")
            .find(".name").text(name).end()
            .find(".name-input").val(name).attr("name", "Comments[" + newIndex + "].Contents").end()
            .find(".selected-input").attr("name", "Comments[" + newIndex + "].IsChecked").val(true).end()
            .appendTo($commentEditor.find(".comment-select"));

        $newCommentName.val("");
        $addCommentButton.pop("disabled", true);
    }

    $addCommentButton.click(function (e) {
        e.preventDefault();
        addComment($addCommentButton.val());
    });

    $newCommentName
        .keyup(function () {
            if ($newCommentName.val().trim().length > 0)
                $addCommentButton.prop("disabled", false);
            else
                $addCommentButton.prop("disabled", true);

        })
        .keydown(function (e) {
            if (e.which !== 13)
                return;

            e.preventDefault();
            addComment($newCommentName.val());

        });



});