var div = document.querySelector('#filter-body');
var icon = document.querySelector('#arrow-icon');
var filter = document.querySelector('.filter-title')
var legend = document.querySelector('.legend-title')
var iconLegend = document.querySelector('#arrow-icon-legend')
var divLegend = document.querySelector('#legend-filter-body')
var open = false;

var findButton = document.querySelector('.find-button')


var url = 'https://localhost:5001/Interviews/AllAjax?'


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
            const nationalities = response.nationalities.find
                (m => m.selected === true);
            console.log(nationalities)
        })
})