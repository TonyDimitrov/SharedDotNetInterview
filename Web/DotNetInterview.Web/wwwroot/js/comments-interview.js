

function handleUserLike() {

    let clickElement = document.getElementById('a-likes-glifi');

    if (clickElement === 'undefined' || clickElement == null) {
        return;
    }
    clickElement.addEventListener('click', updateLike);


    async function updateLike(e) {

        e.preventDefault();

        let url = e.target.parentElement.href;

        const headers = {
            method: 'GET',
            headers: {
                "Content-Type": "application/json"
            },
        }

        await fetch(url, headers)
            .then(handleError)
            .then(serializeData)
            .then(updateLikeUI);
    }

    function updateLikeUI(response) {

        var count = response['count'];
        var css = response['likedCss'];

        var image = document.getElementById('img-like');

        if (!css) {
            image.classList.remove('liked');
        } 
        else {
            image.classList.add(css);
        }
        
        document.getElementById('likes-count').innerText = count;
    }
}

handleUserLike();

// Visualise initial loaded comment
function commentsHandler() {

    const btnShow = "Show";
    const btnHide = "Hide";

    let btnComments = document.querySelector('.btn-comments');

    btnComments.addEventListener('click', displayComments)


    function displayComments(e) {

        let comments = document.getElementsByClassName('div-comment');

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

commentsHandler();

// Add interview comment
function addInterviewComment() {

    const httpMethod = "POST";

    let btnInterviewComment = document.getElementById('form-i-comments');

    if (btnInterviewComment === 'undefined' || btnInterviewComment == null) {
        return;
    }

    btnInterviewComment.addEventListener('submit', createComment);

    async function createComment(e) {
        e.preventDefault();

        let id = e.target.id['value'];
        let content = e.target.content['value'];
        let token = e.target.__RequestVerificationToken['value'];
        let url = e.target.action;

        let dto = {
            id,
            content,
        };

        const headers = {
            method: httpMethod,
            headers: {
                "Content-Type": "application/json",
                "X-CSRF-TOKEN": token
            },
        }
        headers.body = JSON.stringify(dto);

        await fetch(url, headers)
            .then(handleError)
            .then(serializeData)
            .then(buildComments);
    }



    function buildComments(commnets) {

        let divParent = document.getElementsByClassName('div-m')[0];
        let btnAdd = document.getElementsByClassName('div-i-button')[0];

        let oldComments = document.getElementsByClassName('div-i-comment');

        [...oldComments].forEach(oc => divParent.removeChild(oc));
        var fragment = document.createDocumentFragment();

        for (var i = 0; i < commnets.length; i++) {
            let createComment = document.createElement('div');
            createComment.className = "row justify-content-center div-row div-question-border-bottom div-comment div-i-comment";

            let innerContent = `
                    <div class="col-9 col-sm-8 wrap">${commnets[i].content}</div>
                    <div class="col-2 div-small-fond show-over">${commnets[i].modifiedOn}</div>
                    <div class="col-3 col-sm-2 div-small-fond">
                           <a href="/Users/Details?UserId=${commnets[i].userId}" class="a-user-link">
                            ${commnets[i].userFullName}
                        </a>
                        <div class="dis-flex div-small-fond" ${commnets[i].hideDelete}>
                            <form action="/Comments/Delete" method="post" class="form-delete">
                                <div class="form-group mb-2" hidden>
                                    <input class="form-control" name="id" value="${commnets[i].commentId}">
                                </div>
                                <button type="submit" class="btn btn-link a-user-link div-small-fond b-i-delete p-0">Delete</button>
                            </form>
                        </div>
                    </div>
                    </div>`;

            createComment.innerHTML = innerContent;
            fragment.append(createComment);
        }
        divParent.insertBefore(fragment, btnAdd)

        let count = document.getElementsByClassName('div-i-comment').length;
        document.getElementById('comment-count').innerText = `Comments (${count})`;

        let btnSendtextarea = document.getElementById('comment');
        btnSendtextarea.value = "";

        // Add event to delete comment for new created item
        deleteInterviewComment();
    }
}

addInterviewComment();

//   Delete comment logic

function deleteInterviewComment() {

    let [...aDelete] = document.getElementsByClassName('form-delete');

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

        let test = e.target;
        let divComment = test.parentElement.parentElement.parentElement;
        let divParent = document.getElementsByClassName('div-m')[0];
        divParent.removeChild(divComment);

        // Recalculate comments count
        let count = document.getElementsByClassName('div-i-comment').length;
        document.getElementById('comment-count').innerText = `Comments (${count})`;
    }
}

deleteInterviewComment();


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