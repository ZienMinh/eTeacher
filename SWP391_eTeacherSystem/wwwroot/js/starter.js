namespace SWP391_eTeacherSystem.wwwroot.js
{
    /*--------------------------------------------------------------
    # Total Price ClassHour
    --------------------------------------------------------------*/


    /*--------------------------------------------------------------
    # Total Price ClassHour
    --------------------------------------------------------------*/


    /*--------------------------------------------------------------
    # Sweet Alert
    --------------------------------------------------------------*/
    window.addEventListener("load", function) {
        function renderSweetAlert() {
            const template = '<div class="sweet-alert">
                < i class="fa fa-check sweet-icon" ></i >
                    <p class="sweet-text">Successful manipulation</p>
                </div > '
            document.body.insertAdjacentHTML("beforeend", template);
        }
        const button = document.querySelector(".button");
        button.addEventListener("click", function ({
            renderSweetAlert();
        const sweetItem = document.querySelector(".sweet-alert");
        if (sweetItem) {
            setTimeout(function () {
                sweetItem.parentElement.removeChild(sweetItem);
            }, 2000);
        }
    });
        
});

    /*--------------------------------------------------------------
    # Cancel Alert
    --------------------------------------------------------------*/
document.addEventListener('DOMContentLoaded', function () {
    const cancelClassButton = document.getElementById('cancelClassButton');
    const cancelClassForm = document.getElementById('cancelClassForm');

    if (cancelClassButton) {
        cancelClassButton.addEventListener('click', function (event) {
            event.preventDefault();

            const userConfirmed = confirm("Are you sure you want to cancel class?");
            if (userConfirmed) {
                cancelClassForm.submit();
                renderSweetAlert(); // Hiển thị sweet alert khi lớp học bị hủy thành công
            }
        });
    }

    function renderSweetAlert() {
        const template = `
            <div class="sweet-alert">
                <i class="fa fa-check sweet-icon"></i>
                <p class="sweet-text">Class cancelled successfully!</p>
            </div>`;
        document.body.insertAdjacentHTML("beforeend", template);

        const sweetItem = document.querySelector(".sweet-alert");
        if (sweetItem) {
            setTimeout(function () {
                sweetItem.parentElement.removeChild(sweetItem);
            }, 2000);
        }
    }
});

}
