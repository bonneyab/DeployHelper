jQuery(document).ready(function ($) {
    $("#saveStepChanges").click(function () {
        var desc = $("#stepDescription").val();
        var grouping = $("#GroupingId").val();

        var data = { "Order": $('#stepCount').val(), "Description": desc, "GroupingId": grouping };

        $.post("/step/create", data, function (result) {
            $('#stepModal').modal('toggle');
            $('#stepTable tr:last').after('<tr><td>' + result.Order + '</td><td>' + result.Description + '</td><td>false</td><td>Edit | Delete</td></tr>');

            $('#stepCount').val(result.Order + 1);
            //var lastOption = $('#stepOrder option:last');
            //lastOption.after('<option>' + (parseInt(lastOption.val()) + 1) + '</option>');
        }).fail(function() {
             alert("error while saving.");
        });
    });

    $('.step-checkbox').click(function () {
        var stepId = $(this).attr("step-id");
        var complete = $(this).is(':checked');

        var data = { "stepId": stepId, "complete" : complete };
        var rowToUpdate = $(this).parent().parent();
        $.post("/step/complete", data, function () {
            if (complete) {
                rowToUpdate.addClass("success");
            } else {
                rowToUpdate.removeClass("success");
            }
        }).fail(function () {
            alert("error while completing.");
        });
    });

    //TODO: this code needs to gets generisized.
    $(".delete-step").click(function () {
        if (!confirm("Are you sure you want to delete this step?")) {
            return;
        }
        var stepId = $(this).attr("step-id");
        var data = { "stepId": stepId };
        var rowToRemove = $(this).parent().parent();
        $.post("/step/delete", data, function () {
            rowToRemove.remove();
        }).fail(function () {
            alert("error while deleting.");
        });
    });

    $(".edit-step").click(function () {
        var stepId = $(this).attr("step-id");
        $("#stepId").val(stepId);
        var data = { "stepId": stepId };

        //TODO: I really should probably setup a spinner for this sort of stuff.
        $.get("/step/get", data, function(result) {
            $("#editStepDescription").val(result.Description);
            $("#editStepNumber").val(result.Order);
            $("#editStepComplete").val(result.Complete);
        }).fail(function() {
            alert("error while getting step to update.");
        });

        $('#editStepModal').modal('toggle');
    });

    //TODO: these two modals feel a bit funny to me.
    $("#editStepSave").click(function () {
        var desc = $("#editStepDescription").val();
        var number = $("#editStepNumber").val();
        var stepId = $("#stepId").val();
        var grouping = $("#GroupingId").val();
        var complete = $("#editStepComplete").val();

        var data = { "Order": number, "Description": desc, "GroupingId": grouping, "StepId": stepId, "Complete": complete };

        $.post("/step/edit", data, function () {
            $("#stepOrder-" + stepId).html(number);
            $("#stepDescription-" + stepId).html(desc);
            $('#editStepModal').modal('toggle');
        }).fail(function () {
            alert("error while saving.");
        });
    });
});