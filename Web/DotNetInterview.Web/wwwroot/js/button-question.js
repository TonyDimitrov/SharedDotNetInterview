var index = 0;
let indexQuestion = 1;
let questionCount = 2;

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

    let questionDiv = event.target.parentElement.parentElement.parentElement.getElementsByClassName("form-group")[0];
    let div = document.createElement('div');
    div.classList.add("form-group");
    div.classList.add("answer");
    div.innerHTML = `<label for="Questions" class="control-label inner-label">Your answer to this question</label>
                     <textarea name="Questions[${index}].GivenAnswer" rows="2" cols="50" class="form-control"></textarea>
                     <span data-valmsg-for="Questions[${index}].GivenAnswer" class="text-danger"></span>`;
    questionDiv.append(div);
}

function deleteAnswer(event) {

    let questionDiv = event.target.parentElement.parentElement.parentElement;
    questionDiv.getElementsByClassName("answer")[0].remove();
}

setQuestionAnswerBtnListener();

// add question logic

function setQuestion() {

    let btnAddQuestion = document.getElementById("btn-question");

    btnAddQuestion.addEventListener("click", addQuestion);


}

function addQuestion() {


   // let last = Array.from(questionDiv).pop();

    let questionTemplate = `<div class="form-group">
                            <div class="div-textarea">
                                <label for="Questions" class="control-label inner-label">Question</label>
                                <textarea name="Questions[${indexQuestion}].Content" rows="2" cols="50" class="form-control"></textarea>
                                <span asp-validation-for="Questions[${indexQuestion}].Content" class="text-danger"></span>
                            </div>
                            <div>
                            <div class="form-check form-check-inline">
                                <p>You can rank this question as most:</p>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="checkbox" id="inlineCheckbox1" value="option1">
                                    <label class="form-check-label" for="inlineCheckbox1">interesting</label>
                                        </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox2" value="option2">
                                        <label class="form-check-label" for="inlineCheckbox2">unexpected</label>
                                        </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="checkbox" id="inlineCheckbox3" value="option3">
                                            <label class="form-check-label" for="inlineCheckbox3">difficult</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
                                    <div class="btn-group mr-2 btn-answer" role="group" aria-label="First group">
                                        <button type="button" data-add="true"; class="btn btn-sm btn-secondary ">Add answer</button>
                                    </div>
                                    <div class="btn-group mr-2 question-delete" role="group" aria-label="Third group">
                                       <button type="button" class="btn btn-sm btn-secondary">Delete question</button>
                                    </div>
                                    <div class="btn-group mr-2" role="group" aria-label="Third group">
                                       <button type="button" class="btn btn-sm btn-secondary">Upload task</button>
                                    </div>
                              </div>`;

    let div = document.createElement('div');
    div.className = "question";
    div.innerHTML = questionTemplate;

    let questionDiv = document.getElementsByClassName("question");
    questionDiv = questionDiv[questionDiv.length - 1];
    questionDiv.insertAdjacentElement("afterend", div);
    questionCount++;
    indexQuestion++;
    setQuestionAnswerBtnListener();
    addDeleteEventListener();
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
