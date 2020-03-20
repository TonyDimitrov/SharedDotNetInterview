

const unexpectedType = "unexpected";
const interestedType = "interesting";
const difficultType = "difficult";

// add location logic 

function addEventToLocationType() {
    var btnRadio = document.getElementsByClassName("btn-radio");

    [...btnRadio].forEach(r => r.addEventListener("change", manageLocationInput));
}

function manageLocationInput() {

    var radious = document.getElementById("office-type");
    let divLocation = document.getElementById("div-location");

    if (radious.checked) {

        divLocation.hidden = false;
    }
    else {

        divLocation.hidden = true;
    }
}

addEventToLocationType();
// add answer logic

function setQuestionAnswerBtnListener() {

    let btnsAnswer = document.getElementsByClassName("btn-answer");

    [...btnsAnswer].forEach(answer => {
        answer.addEventListener('click', handleQuestionAnswer);
    });
}


function handleQuestionAnswer(event) {

    let shouldAdd = event.target.getAttribute("data-add") === "true";

    if (shouldAdd) {
        addAnswer(event)
        event.target.innerText = "Delete answer";
        event.target.dataset.add = "false";
    }
    else {
        deleteAnswer(event);
        event.target.innerText = "Add answer";
        event.target.dataset.add = "true";
    }
}

function addAnswer(event) {

    let questionDiv = event.target
        .parentElement.parentElement
        .parentElement
        .getElementsByClassName("form-group")[0];

    let currentIndex = questionDiv.dataset.index;
    let currentNumber = questionDiv.dataset.number;

    let div = document.createElement('div');
    div.classList.add("form-group");
    div.classList.add("answer");
    div.innerHTML = `<label for="Questions" class="control-label inner-label">Answer for question ${currentNumber}</label>

                     <textarea rows="2" cols="50" class="form-control" data-val="true"
                     data-val-maxlength="Question content should have maximum 5000 characters!"
                     data-val-maxlength-max="5000" data-val-minlength="Answer content should have minimum 2 characters!"
                     data-val-minlength-min="2" data-val-required="Answer content is required!" 
                     id="Questions_${currentIndex}__GivenAnswer" maxlength="5000" name="Questions[${currentIndex}].GivenAnswer"></textarea>

                     <span class="text-danger field-validation-valid" data-valmsg-for="Questions[${currentIndex}].GivenAnswer" data-valmsg-replace="true"></span>`;

    questionDiv.append(div);

    $('form').data('validator', null);
    $.validator.unobtrusive.parse($('form'));
}

function deleteAnswer(event) {

    let questionDiv = event.target.parentElement.parentElement.parentElement;
    questionDiv.getElementsByClassName("answer")[0].remove();
}

function updateQuestionAndAnswerIndex() {
    var questions = document.getElementsByClassName("question-group");

    for (var i = 0; i < [...questions].length; i++) {
        var content = questions[i].innerHTML;
        content = content.replace("${index}");
    }
}

setQuestionAnswerBtnListener();

// add question logic

function setQuestion() {

    let btnAddQuestion = document.getElementById("btn-question");

    btnAddQuestion.addEventListener("click", addQuestion);


}

function addQuestion() {

    var index = document.getElementsByClassName("question-group").length;
    var number = index + 1;

    let questionTemplate = `<div class="form-group question-group" data-index="${index}" data-number="${number}">
                            <div class="div-textarea">
                                <label for="Questions" class="control-label inner-label required">${number} Question</label>
                                 <textarea rows="2" cols="50" class="form-control" data-val="true"
                                 data-val-maxlength="Question content should have maximum 1000 characters!"
                                 data-val-maxlength-max="1000" data-val-minlength="Question content should have minimum 2 characters!"
                                 data-val-minlength-min="2" data-val-required="Question content is required!" 
                                 id="Questions_${index}__Content" maxlength="1000" name="Questions[${index}].Content"></textarea>
                                <span class="text-danger field-validation-valid" data-valmsg-for="Questions[${index}].Content" data-valmsg-replace="true"></span>
                            </div>
                            <div>
                            <div class="form-check form-check-inline">
                                <p>You can rank this question as most:</p>
                            </div>
                                    <div class="form-check form-check-inline">
                                         <input class="form-check-input interesting" type="checkbox" name="Questions[${index}].Interesting" value="1">
                                         <label class="form-check-label" for="inlineCheckbox1">interesting</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                         <input class="form-check-input unexpected" type="checkbox" name="Questions[${index}].Unexpected" value="2">
                                         <label class="form-check-label" for="inlineCheckbox2">unexpected</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                            <input class="form-check-input difficult" type="checkbox" name="Questions[${index}].Difficult" value="3">
                                            <label class="form-check-label" for="inlineCheckbox3">difficult</label>
                                    </div>
                            </div>
                                </div>
                                <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
                                    <div class="btn-group mr-2 btn-answer" role="group" aria-label="First group">
                                        <button type="button" data-add="true"; class="btn btn-sm btn-secondary ">Add answer</button>
                                    </div>
                                     <div class="btn-group">
                                        <input class="input-file file" for="customFile" type="file" name="Questions[${index}].formFile">
                                     </div>
                              </div>`;

    let div = document.createElement('div');
    div.className = "question";
    div.innerHTML = questionTemplate;

    let questionDiv = document.getElementsByClassName("question");
    questionDiv = questionDiv[questionDiv.length - 1];
    questionDiv.insertAdjacentElement("afterend", div);

    setQuestionAnswerBtnListener();
    addDeleteEventListener();
    addCheckboxEvent();
    disableCheckbox(unexpectedType);
    disableCheckbox(interestedType);
    disableCheckbox(difficultType);
    addEventOnInputButton();

    $('form').data('validator', null);
    $.validator.unobtrusive.parse($('form'));
}

setQuestion();

// add delete question logic

function addDeleteEventListener() {

    var btnDeleteQuestion = document.getElementsByClassName("question-delete");

    [...btnDeleteQuestion].forEach(btnDel => btnDel.addEventListener("click", deleteQuestion))

}

function deleteQuestion(event) {

    let divQuestion = event.target.parentElement.parentElement.parentElement;
    divQuestion.remove();

}

addDeleteEventListener();

// add checkbox logic here

function addCheckboxEvent() {

    let checkboxes = document.getElementsByClassName("form-check-input");

    [...checkboxes].forEach(ch => ch.addEventListener("change", disableCheckboxOnEvent));
}

function disableCheckboxOnEvent(event) {

    disableCheckboxByType(event, unexpectedType);
    disableCheckboxByType(event, interestedType);
    disableCheckboxByType(event, difficultType);

}

function disableCheckboxByType(event, checkboxType) {

    if ([...event.target.classList].includes(checkboxType) && event.target.checked) {

        var currentCheckbox = event.target;

        var allUnexpected = document.getElementsByClassName(checkboxType);

        [...allUnexpected].forEach(un => un.disabled = true);

        currentCheckbox.disabled = false;
    }
    else if ([...event.target.classList].includes(checkboxType) && !event.target.checked) {

        var allUnexpected = document.getElementsByClassName(checkboxType);

        [...allUnexpected].forEach(un => un.disabled = false);
    }
}

function disableCheckbox(checkboxType) {

    var checkboxUnexpected = document.getElementsByClassName(checkboxType);
    var checked = [...checkboxUnexpected].filter(ch => ch.checked);

    if (checked.length > 0) {
        [...checkboxUnexpected].forEach(un => un.disabled = true);
        [...checked][0].disabled = false;
    }

}

addCheckboxEvent();

// file upload logic

function addEventOnInputButton() {

    let btns = document.getElementsByClassName("input-file");

    [...btns].forEach(btn => btn.addEventListener('load', clickFileButton))
}

function clickFileButton(event) {

    var removeBtn = event.target.parentElement.getElementsByClassName('btn-remove-file');

    [...removeBtn][0].removeAttribute("hidden");

    var fileLabel = event.target.parentElement.parentElement.getElementsByClassName('label-file');

    Promise.resolve([...file][0].click()).then(function () {
        [...fileLabel][0].innerText = [...file][0].value;
    });
}
