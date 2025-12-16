$(function () {
    var l = abp.localization.getResource("Exam");
	
	var challengeUserTotalService = window.exam.challengeUserTotals.challengeUserTotals;
	
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
        viewUrl: abp.appPath + "ChallengeUserTotals/CreateModal",
        scriptUrl: abp.appPath + "Pages/ChallengeUserTotals/createModal.js",
        modalClass: "challengeUserTotalCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "ChallengeUserTotals/EditModal",
        scriptUrl: abp.appPath + "Pages/ChallengeUserTotals/editModal.js",
        modalClass: "challengeUserTotalEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            totalValueMin: $("#TotalValueFilterMin").val(),
			totalValueMax: $("#TotalValueFilterMax").val(),
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
                                visible: abp.auth.isGranted('Exam.ChallengeUserTotals.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.challengeUserTotal.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Exam.ChallengeUserTotals.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    challengeUserTotalService.delete(data.record.challengeUserTotal.id)
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
			{ data: "challengeUserTotal.totalValue" },
            {
                data: "challenge.name",
                defaultContent : ""
            },
            {
                data: "identityUser.userName",
                defaultContent : ""
            }        
    ];
    
    

    var dataTable = $("#ChallengeUserTotalsTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(challengeUserTotalService.getList, getFilter),
        columnDefs: dataTableColumns
    }));
    
    

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewChallengeUserTotalButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        challengeUserTotalService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/challenge-user-totals/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText },
                            { name: 'totalValueMin', value: input.totalValueMin },
                            { name: 'totalValueMax', value: input.totalValueMax }, 
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
