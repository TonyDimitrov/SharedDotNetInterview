
function commentsHandler2() {

    const btnShow = "Show";
    const btnHide = "Hide";

    let btnComments = document.getElementsByClassName('btn-comments2');

    [...btnComments].forEach(c => c.addEventListener('click', displayComments));


    function displayComments(e) {

        let divCommentsWrapper = e.target.parentElement.parentElement.parentElement;

        let comments = divCommentsWrapper.getElementsByClassName('div-comment2');

        let btnTitleCurrentText = e.target.innerText;

        if (btnTitleCurrentText === btnShow) {

            [...comments].forEach(c => c.hidden = false);
            e.target.innerText = btnHide;
        }
        else if (btnTitleCurrentText === btnHide) {

            [...comments].forEach(c => c.hidden = true);
            e.target.innerText = btnShow;
        }
    }
}

commentsHandler2();


function addQuestionComment() {

    const httpMethod = "POST";

    let btnQuestionComments = document.getElementsByClassName('form-q-comments');

    [...btnQuestionComments].forEach(fc => fc.addEventListener('submit', createComment));

    async function createComment(e) {
        e.preventDefault();

        let id = e.target.id['value'];
        let content = e.target.content['value'];
        let url = e.target.action;


        let dto = {
            id,
            content,
        };

        const headers = {
            method: httpMethod,
            headers: {
                "Content-Type": "application/json"
            },
        }
        headers.body = JSON.stringify(dto);

        await fetch(url, headers)
            .then(handleError)
            .then(serializeData)
            .then(buildComments.bind(null, e.target));
    }

    function buildComments(form, comments) {

        let divParent = form
            .parentElement
            .parentElement
            .parentElement
            .parentElement
            .getElementsByClassName('div-m2')[0];

        let btnAdd = form
            .parentElement
            .parentElement
            .parentElement
            .getElementsByClassName('div-q-button')[0];

        let oldComments = form
            .parentElement
            .parentElement
            .parentElement.getElementsByClassName('div-q-comment');

        [...oldComments].forEach(oc => divParent.removeChild(oc));
        var fragment = document.createDocumentFragment();

        for (var i = 0; i < comments.length; i++) {
            let createComment = document.createElement('div');
            createComment.className = "row div-row div-r-bb div-comment2 div-q-comment";

            let innerContent = `
                    <div class="col-8">${comments[i].content}</div>
                    <div class="col-2 div-small-fond">${comments[i].modifiedOn}</div>
                    <div class="col-2 div-small-fond">
                           <a href="/Users/Details?UserId=${comments[i].userId}" class="a-user-link">
                            ${comments[i].userFullName}
                        </a>
                        <div class="dis-flex div-small-fond" ${comments[i].hideDelete}>
                            <form action="/Comments/Delete" method="post" class="form-delete2">
                                <div class="form-group mb-2" hidden>
                                    <input class="form-control" name="id" value="${comments[i].commentId}">
                                </div>
                                <button type="submit" class="btn btn-link a-user-link div-small-fond b-i-delete p-0">Delete</button>
                            </form>
                        </div>
                    </div>`;

            createComment.innerHTML = innerContent;
            fragment.append(createComment);
        }
        divParent.insertBefore(fragment, btnAdd)

        let count = comments.length;

        divParent.getElementsByClassName('comment-count2')[0].innerText = `Comments (${count})`;

        let btnSendtextarea = form.getElementsByClassName('comment2')[0];
        btnSendtextarea.value = "";

        // Add event to delete comment for new created item
        deleteQuestionComment();
    }
}

addQuestionComment();

//   Delete comment logic

function deleteQuestionComment() {

    let [...aDelete] = document.getElementsByClassName('form-delete2');

    if (!aDelete) {
        return;
    }

    aDelete.forEach(fd => fd.addEventListener('submit', deleteComment));

    async function deleteComment(e) {
        e.preventDefault();

        const httpMethod = "POST";

        let url = e.target.action;
        let id = e.target.id['value'];
        const headers = {
            method: httpMethod,
            headers: {
                "Content-Type": "application/json"
            },
        }
        headers.body = JSON.stringify(id);

        await fetch(url, headers)
            .then(handleError)
            .then(serializeData)
            .then(deleteCommentElement.bind(null, e));
    }

    function deleteCommentElement(e, responce) {

        let form = e.target;
        let divComment = form.parentElement.parentElement.parentElement;
        let divParent = form
            .parentElement
            .parentElement
            .parentElement
            .parentElement
            .parentElement
            .getElementsByClassName('div-m2')[0];

        let count = --form
            .parentElement
            .parentElement
            .parentElement
            .parentElement
            .getElementsByClassName('div-q-comment').length;

        divParent.removeChild(divComment);

        // Recalculate comments count     

        divParent.getElementsByClassName('comment-count2')[0].innerText = `Comments (${count})`;
    }
}

deleteQuestionComment();

// Helper functions

function handleError(e) {
    if (!e.ok) {
        throw new Error(e.statusText);
    }

    return e;
}

function serializeData(x) {
    if (x.status === 204) {
        return x;
    }
    return x.json();
}