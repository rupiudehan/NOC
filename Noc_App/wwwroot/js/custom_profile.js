function submitForm(anchor, event) {
    if ($(anchor).hasClass('activediv')) {
        event.preventDefault();

        // Get the anchor element
        var anchort = event.target;

        // Add the 'disabled' class
        anchort.classList.add('disabled');

        // Remove the 'onclick' event to disable further clicks
        anchort.onclick = null;
    } else {
        $(anchor).find('#postDataForm').submit();
    }
}