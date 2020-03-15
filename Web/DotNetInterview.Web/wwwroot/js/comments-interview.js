
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

    const httpMethod = "POST";

    let btnInterviewComment = document.getElementById('form-i-comments');

    btnInterviewComment.addEventListener('submit', createComment);

    async function createComment(e) {
        e.preventDefault();

        let id = e.target.Id['value'];
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

    function buildComments(commnets) {
        let obj = commnets;
        let divParent = document.getElementsByClassName('div-m')[0];
        let btnSend = document.getElementsByClassName('div-i-button')[0];

        let oldComments = document.getElementsByClassName('div-i-comment');

        [...oldComments].forEach(oc => divParent.removeChild(oc));
        var fragment = document.createDocumentFragment();

        for (var i = 0; i < commnets.length; i++) {
            let createComment = document.createElement('div');
            createComment.className = "row justify-content-center div-row div-r-bb div-comment div-i-comment";

            let innerContent = `
                    <div class="col-9">${commnets[i].content}</div>
                    <div class="col-2 div-small-fond">${commnets[i].modifiedOn}</div>
                    <div class="col-1 div-small-fond">
                           <a href="/Users/Details?UserId=${commnets[i].userId}" class="a-user-link">
                            ${commnets[i].userFullName}
                        </a>
                    </div>`;

            createComment.innerHTML = innerContent;
            fragment.append(createComment);
        }
        divParent.insertBefore(fragment, btnSend)

        let count = document.getElementsByClassName('div-i-comment').length;
        document.getElementById('comment-count').innerText = `Comments (${count})`;

        let btnSendtextarea = document.getElementById('comment');
        btnSendtextarea.value = "";
    }
}

addInterviewComment();