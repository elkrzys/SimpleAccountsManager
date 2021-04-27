/**
 * Function displaying confirmation pop-up when user is trying to delete account from list.
 * When user confirms, the function redirects to action Delete of the Content controller.
 * @param {String} name account name
 * @param {String} id   encrypted id of the account
 */
ConfirmDelete = (name, id) => {
    let url = '/Content/Delete?id=';
    if (confirm("Delete " + name + "?"))
        window.location.href = url + id;
    else
        return false;
}