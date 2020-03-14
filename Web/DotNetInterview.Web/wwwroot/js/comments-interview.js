
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


function addInterviewComment() {

    const httpMethod = "post";

    let btnInterviewComment = document.getElementById('form-i-comments');

    btnInterviewComment.addEventListener('submit', createComment);

    async function createComment(e) {
        e.preventDefault();

        let id = e.target.interviewId['value'];
        let content = e.target.content['value'];
        let url = e.target.action;


        let dto = {
            interviewId: id,
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
            .then(buildComments);
    }

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

    function buildComments(x) {
        let obj = x;
        let divParent = document.getElementsByClassName('div-m')[0];
        let btnSend = document.getElementsByClassName('div-i-button')[0];
        let createComment = document.createElement('div');
        createComment.className = "row justify-content-center div-row div-r-bb div-comment div-i-comment";

        let innerContent = `
                <div class="col-8">${x[0].content}</div>
                <div class="col-4">${x[0].createdOn}</div>`;

        createComment.innerHTML = innerContent;
        divParent.insertBefore(createComment, btnSend);

        let count = document.getElementsByClassName('div-i-comment').length;
        document.getElementById('comment-count').innerText = `Comments (${count})`;

        let btnSendtextarea = document.getElementById('comment');
        btnSendtextarea.value = "";
    }
}

addInterviewComment();