var index = 0;

function setQuestionAnswer() {

    let btnsAnswer = document.getElementsByClassName("btn-answer");

    [...btnsAnswer].forEach(answer => {
        answer.addEventListener('click', handleQuestionAnswer);
    });
}

function handleQuestionAnswer(event) {

    var shouldAdd = event.target.getAttribute("data-add") === "true";

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

    var questionDiv = event.target.parentElement.parentElement.parentElement.getElementsByClassName("form-group")[0];
    var div = document.createElement('div');
    div.classList.add("form-group");
    div.classList.add("answer");
    div.innerHTML = `<label for="Questions" class="control-label">Your answer to this question</label>
                     <textarea name="Questions[${index}].GivenAnswer" rows="2" cols="50" class="form-control"></textarea>
                     <span data-valmsg-for="Questions[${index}].GivenAnswer" class="text-danger"></span>`;
    questionDiv.append(div);
}

function deleteAnswer(event) {

    var questionDiv = event.target.parentElement.parentElement.parentElement;
    questionDiv.getElementsByClassName("answer")[0].remove();
}

setQuestionAnswer();


