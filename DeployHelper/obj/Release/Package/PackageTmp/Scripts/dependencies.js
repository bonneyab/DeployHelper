jQuery(document).ready(function ($) {
    $("#saveDependencyChanges").click(function () {
        var dependencyId = $("#dependencyOptions").val();
        var grouping = $("#GroupingId").val();

        var data = { "DependentOnGroupingId": dependencyId, "DependentGroupingId": grouping };

        $.post("/dependency/create", data, function (result) {
            $('#dependencyModal').modal('toggle');

            var vendorActionRequired = result.VendorActionRequired ? result.VendorActionComplete : "N/A";
            $('#dependencyTable tr:last').after('<tr><td>' + result.Name + '</td><td>' + result.Owner + '</td><td>' + result.Completed + '</td><td>' + vendorActionRequired + '</td><td>Delete</td></tr>');

            $('#dependencyCount').val(result.Order + 1);
            var itemToRemove = '#dependencyOptions option[value="' + dependencyId + '"]';
            $(itemToRemove).remove();
        }).fail(function() {
             alert("error while saving.");
        });
    });

    $(".delete-dependency").click(function () {
        if (!confirm("Are you sure you want to delete this dependency?")) {
            return;
        }
        var dependentOnId = $(this).attr("dependency-id");
        var dependentId = $("#GroupingId").val();
        var data = { "dependentOnId": dependentOnId, "dependentId": dependentId };
        
        var rowToRemove = $(this).parent().parent();
        $.post("/dependency/delete", data, function (result) {
            rowToRemove.remove();
            $('#dependencyOptions').append($('<option></option>').val(result.GroupingId).html(result.Name));
        }).fail(function () {
            alert("error while deleting.");
        });
    });
});