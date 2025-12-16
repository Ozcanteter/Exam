$(function () {
    var l = abp.localization.getResource("Exam");
	
	var challengeService = window.exam.challenges.challenges;
	
	
    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Challenges/CreateModal",
        scriptUrl: abp.appPath + "Pages/Challenges/createModal.js",
        modalClass: "challengeCreate"
    });

	var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Challenges/EditModal",
        scriptUrl: abp.appPath + "Pages/Challenges/editModal.js",
        modalClass: "challengeEdit"
    });

	var getFilter = function() {
        return {
            filterText: $("#FilterText").val(),
            name: $("#NameFilter").val(),
			startDateMin: $("#StartDateFilterMin").val(),
			startDateMax: $("#StartDateFilterMax").val(),
			endDateMin: $("#EndDateFilterMin").val(),
			endDateMax: $("#EndDateFilterMax").val(),
			goalMin: $("#GoalFilterMin").val(),
			goalMax: $("#GoalFilterMax").val(),
            isActive: (function () {
                var value = $("#IsActiveFilter").val();
                if (value === undefined || value === null || value === '') {
                    return '';
                }
                return value === 'true';
            })()
        };
    };
    
    
    
    var dataTableColumns = [
            {
                rowAction: {
                    items:
                        [
                            {
                                text: l("Edit"),
                                visible: abp.auth.isGranted('Exam.Challenges.Edit'),
                                action: function (data) {
                                    editModal.open({
                                     id: data.record.id
                                     });
                                }
                            },
                            {
                                text: l("Delete"),
                                visible: abp.auth.isGranted('Exam.Challenges.Delete'),
                                confirmMessage: function () {
                                    return l("DeleteConfirmationMessage");
                                },
                                action: function (data) {
                                    challengeService.delete(data.record.id)
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
			{ data: "name" },
            {
                data: "startDate",
                render: function (startDate) {
                    if (!startDate) {
                        return "";
                    }
                    
					var date = Date.parse(startDate);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
            {
                data: "endDate",
                render: function (endDate) {
                    if (!endDate) {
                        return "";
                    }
                    
					var date = Date.parse(endDate);
                    return (new Date(date)).toLocaleDateString(abp.localization.currentCulture.name);
                }
            },
			{ data: "goal" },
            {
                data: "isActive",
                render: function (isActive) {
                    return isActive ? '<i class="fa fa-check"></i>' : '<i class="fa fa-times"></i>';
                }
            }        
    ];
    
    

    var dataTable = $("#ChallengesTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        scrollX: true,
        autoWidth: true,
        scrollCollapse: true,
        order: [[1, "asc"]],
        ajax: abp.libs.datatables.createAjax(challengeService.getList, getFilter),
        columnDefs: dataTableColumns
    }));
    
    

    createModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();;
    });

    $("#NewChallengeButton").click(function (e) {
        e.preventDefault();
        createModal.open();
    });

	$("#SearchForm").submit(function (e) {
        e.preventDefault();
        dataTable.ajax.reloadEx();;
    });

    $("#ExportToExcelButton").click(function (e) {
        e.preventDefault();

        challengeService.getDownloadToken().then(
            function(result){
                    var input = getFilter();
                    var url =  abp.appPath + 'api/app/challenges/as-excel-file' + 
                        abp.utils.buildQueryString([
                            { name: 'downloadToken', value: result.token },
                            { name: 'filterText', value: input.filterText }, 
                            { name: 'name', value: input.name },
                            { name: 'startDateMin', value: input.startDateMin },
                            { name: 'startDateMax', value: input.startDateMax },
                            { name: 'endDateMin', value: input.endDateMin },
                            { name: 'endDateMax', value: input.endDateMax },
                            { name: 'goalMin', value: input.goalMin },
                            { name: 'goalMax', value: input.goalMax }, 
                            { name: 'isActive', value: input.isActive }
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
