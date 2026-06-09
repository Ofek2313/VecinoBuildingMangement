let selectedOption = null;
let selectedPoll = null;
let selectedOptionEl = null;

function select(button, optionId, pollId) {
    if (selectedOptionEl) selectedOptionEl.style.backgroundColor = "";
    button.style.backgroundColor = "#d1e7ff";
    selectedOptionEl = button;
    selectedOption = optionId;
    selectedPoll = pollId;
}

async function vote(pollId, button) {
    if (selectedOption == null || selectedPoll == null) {
        alert("Select an option")
        return;
    }

    try {
        const res = await fetch(`http://localhost:5086/Resident/VoteInPoll`,
            {
                method: 'Post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ OptionId: selectedOption, PollId: selectedPoll })
            });
        const result = await res.json();
        if (result.success == true) {
            handleData(result.data, pollId);
            showResults();
        }
        else {
            alert("Error While Voting, Vote Was Rejected")
        }
    }
    catch (err) {
        console.error(err);
        alert(err.message || 'Could not vote');
    }
}

function handleData(data, pollId) {
    data.forEach(opt => {
        const optionId = opt.option.optionId;
        const percentage = opt.percentage;
        const resultContainer = document.getElementById(`result-${pollId}`);
        const progressBar = resultContainer.querySelector(`.option-progress[data-optionid="${optionId}"]`);
        progressBar.style.width = percentage + '%';
        const percentageText = document.getElementById(`option-${optionId}`);
        percentageText.innerText = percentage + '%';
    })
}

function showResults() {
    document.getElementById(`result-${selectedPoll}`).style.display = 'block';
    document.getElementById(`vote-${selectedPoll}`).style.display = 'none';
}

async function unVote(pollId, button) {
    const res = await fetch(`http://localhost:5086/Resident/UnVoteInPoll?pollId=${pollId}`,
        {
            method: 'Post',
        });
    const data = await res.json();

    if (data.success) {
        document.getElementById(`result-${pollId}`).style.display = 'none';
        document.getElementById(`vote-${pollId}`).style.display = 'block';

        selectedOption = null;
        selectedPoll = null;

        if (selectedOptionEl) {
            selectedOptionEl.style.backgroundColor = "";
            selectedOptionEl = null;
        }
    }
}
function clearButtons() {
    const filterButtons = document.querySelectorAll(".filter-btn");
    filterButtons.forEach(button => {
        button.classList.remove("filter-btn--active");
    });
}
function filterAll() {
    const pollCards = document.querySelectorAll(".poll-card");
    pollCards.forEach(card => {
        card.style.display = "block";

    });
    clearButtons();
    const button = document.getElementById("filter-all");
    button.classList.add("filter-btn--active");
}
function filterOpen() {
    const pollCards = document.querySelectorAll(".poll-card");
    pollCards.forEach(card => {
        card.style.display = "block";
        const cardStatus = card.getAttribute("data-status");
        if (cardStatus == "Active")
            card.style.display = "block";
        else
            card.style.display = "none";
    });
    clearButtons();
    const button = document.getElementById("filter-open");
    button.classList.add("filter-btn--active");

}
function filterClose() {
    const pollCards = document.querySelectorAll(".poll-card");
    pollCards.forEach(card => {
        card.style.display = "block";
        const cardStatus = card.getAttribute("data-status");
        if (cardStatus == "Closed")
            card.style.display = "block";
        else
            card.style.display = "none";
    });
    clearButtons();
    const button = document.getElementById("filter-closed");
    button.classList.add("filter-btn--active");
}