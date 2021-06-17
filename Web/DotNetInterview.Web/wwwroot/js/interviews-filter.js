var div = document.querySelector('#filter-body');
var icon = document.querySelector('#arrow-icon');
var filter = document.querySelector('.filter-title')
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