let currentFeeId = null;
function OpenPopUp(feeId) {
    const overlay = document.getElementById('overlay');
    overlay.style.display = 'flex';
    currentFeeId = feeId;

}
async function Payfee() {
    try {

        const res = await fetch(`http://localhost:5086/Resident/PayFee`,
            {
                method: 'Post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ FeeId: currentFeeId })

            });
        const data = await res.json();
        if (data.success == true) {
            location.reload();
            currentFeeId = null;
        }
        else {
            alert("Could not pay fee");
        }


    }
    catch (err) {
        console.error(err);
        alert(err.message || 'Could not attend event');
    }
}