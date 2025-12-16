$(function () {
    var l = abp.localization.getResource("Exam");
	
	var progressEntryService = window.exam.progressEntries.progressEntries;
	
        var lastNpIdId = '';
        var lastNpDisplayNameId = '';

        var _lookupModal = new abp.ModalManager({
            viewUrl: abp.appPath + "Shared/LookupModal",
            scriptUrl: abp.appPath + "Pages/Shared/lookupModal.js",
            modalClass: "navigationPropertyLookup"
        });

        $('.lookupCleanButton').on('click', '', function () {
            $(this).parent().find('input').val('');
        });

        _lookupModal.onClose(function () {
            var modal = $(_lookupModal.getModal());
            $('#' + lastNpIdId).val(modal.find('#CurrentLookupId').val());
            $('#' + lastNpDisplayNameId).val(modal.find('#CurrentLookupDisplayName').val());
        });
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ProgressEntries/CreateModal",
        scriptUrl: abp.appPath + "Pages/ProgressEntries/createModal.js",
        modalClass: "progressEntryCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ProgressEntries/EditModal",
        scriptUrl: abp.appPath + "Pages/ProgressEntries/editModal.js",
        modalClass: "progressEntryEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            valueMin: $("#ValueFilterMin").val(),
			valueMax: $("#ValueFilterMax").val(),
			challengeId: $("#ChallengeIdFilter").val(),			identityUserId: $("#IdentityUserIdFilter").val()
        };
    };
    
    
    
    var dataTableColumns = [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Exam.ProgressEntries.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.progressEntry.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Exam.ProgressEntries.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    progressEntryService.delete(data.record.progressEntry.id)
                                        .then(function () {
                                            abp.notify.info(l("SuccessfullyDeleted"));
                                            dataTable.ajax.reloadEx();;
                                        });
                                }
                            }
                        ]
                },
                width: "1rem"
            },
			{ data: "progressEntry.value" },
            {
                data: "challenge.name",
                defaultContent : ""
            },
            {
                data: "identityUser.userName",
                defaultContent : ""
            }        
    ];
    
    

    var dataTable = $("#ProgressEntriesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(progressEntryService.getList, getFilter),
        columnDefs: dataTableColumns
    }));
    
    

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewProgressEntryButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        progressEntryService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/progress-entries/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText },
                            { name: 'valueMin', value: input.valueMin },
                            { name: 'valueMax', value: input.valueMax }, 
                            { name: 'challengeId', value: input.challengeId }
, 
                            { name: 'identityUserId', value: input.identityUserId }
                            ]);
                            
                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
            }
        )
    });

    $('#AdvancedFilterSectionToggler').on('click', function (e) {
        $('#AdvancedFilterSection').toggle();
    });

    $('#AdvancedFilterSection').on('keypress', function (e) {
        if (e.which === 13) {
            dataTable.ajax.reloadEx();;
        }
    });

    $('#AdvancedFilterSection select').change(function() {
        dataTable.ajax.reloadEx();;
    });
    
    
    
    
    
    
    
    
});
