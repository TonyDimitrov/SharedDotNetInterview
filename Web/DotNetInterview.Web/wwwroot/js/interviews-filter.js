var div = document.querySelector('#filter-body');
var icon = document.querySelector('#arrow-icon');
var filter = document.querySelector('.filter-title')
var legend = document.querySelector('.legend-title')
var iconLegend = document.querySelector('#arrow-icon-legend')
var divLegend = document.querySelector('#legend-filter-body')
var open = false;

var findButton = document.querySelector('.find-button')

var showAllBtn = document.querySelector('.show-all-button')

var url = '/Interviews/AllAjax?'


filter.addEventListener('click', function () {
    if (open) {
        icon.className = 'fa fa-arrow-down';
        div.className = 'hide';
    } else {
        icon.className = 'fa fa-arrow-down open';
        div.className = '';
    }

    open = !open;
});

legend.addEventListener('click', function () {
    if (open) {
        iconLegend.className = 'fa fa-arrow-down';
        divLegend.className = 'hide';
    } else {
        iconLegend.className = 'fa fa-arrow-down open';
        divLegend.className = '';
    }

    open = !open;
});

findButton.addEventListener('click', function (e) {
    e.preventDefault()

    var nationalityCompany = document.querySelector('.nationality-selection').value
    var seniorityLevel = document.querySelector('.seniority-selection').value
    var startDate = document.querySelector('#start-date').value
    var endDate = document.querySelector('#end-date').value

    var interviewsResultBox = document.querySelector('.interviews-result-box')
    interviewsResultBox.innerHTML = ''
    let interviewItem = ''

    console.log(e)

    var finalURL = ''
    var queryParams = ''
    var query = {}

    Object.assign(query, { 'nationalityId': nationalityCompany })
    Object.assign(query, { 'seniority': seniorityLevel })
    Object.assign(query, { 'from': startDate })
    Object.assign(query, { 'to': endDate })

    for (let kvp of Object.entries(query)) {
        if (!kvp[1] == '') {
            queryParams += (kvp[0] + '=' + kvp[1] + '&')
        }
    }

    queryParams = queryParams.slice(0, -1)
    finalURL = url + queryParams
    console.log(finalURL)

    fetch(finalURL)
        .then(response => response.json())
        .then(response => {
            console.log(response)

            response.interviews.map(item => {
                interviewItem = `
                    <div class="row div-row div-question-border-bottom">
                         <div class="col-10 col-sm-3">${item.positionTitle}</div>
                         <div class="d-none d-sm-block col-sm-2 seniority-wrapper">
                              <img class="seniority" src="/img/${item.seniority}">
                              <span class="seniority-tooltiptext">${item.seniorityTooltip}</span>
                        </div>
                        <div class="col-1 col-sm-1"><span class="badge badge-pill span-items-count">${item.questions}</span></div>
                        <div class="d-none d-sm-block col-sm-2">${item.date}</div>
                        <div class="d-none d-sm-block col-sm-1"><span class="badge badge-pill span-items-count">${item.likes}</span></div>
                        <div class="d-none d-sm-block col-sm-2">
                            <a class="a-user" href="/Users/Details?userId=${item.creatorId}" target="_new">
                                ${item.creatorName}
                            </a>
                        </div>
                        <div class="col-12 col-sm-1 btn-detail">
                            <a class="btn btn-sm btn-primary" href="/Interviews/Details?interviewId=${item.interviewId}" target="_new">Details</a>
                        </div>
                    </div>
                `
                interviewsResultBox.innerHTML += interviewItem
            })
        })
})