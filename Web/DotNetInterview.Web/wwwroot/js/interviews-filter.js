var div = document.querySelector('#filter-body');
var icon = document.querySelector('#arrow-icon');
var filter = document.querySelector('.filter-title')
var legend = document.querySelector('.legend-title')
var iconLegend = document.querySelector('#arrow-icon-legend')
var divLegend = document.querySelector('#legend-filter-body')
var open = false;

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

