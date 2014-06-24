jQuery(document).ready(function ($) {
    $(".delete-grouping").click(function () {
        if (!confirm("Are you sure you want to delete this grouping?")) {
            return;
        }
        var groupingId = $(this).attr("grouping-id");
        
        var data = { "groupingId": groupingId };
        var rowToRemove = $(this).parent().parent();
        $.post("/groupings/delete", data, function () {
            rowToRemove.remove();
        }).fail(function () {
            alert("error while deleting.");
        });
    });
});