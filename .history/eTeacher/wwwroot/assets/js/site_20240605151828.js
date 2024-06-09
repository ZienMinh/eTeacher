const uri = "api/Auth/register";
let users = [];

function getUsers() {
  fetch(uri)
    .then((response) => response.json())
    .then((data) => _displayUsers(data))
    .catch((error) => console.error("Unable to get items.", error));
}

function addUsers() {
  const addUsernameTextbox = document.getElementById("add-username");
  const addFirstnameTextbox = document.getElementById("add-firstname");
  const addLastnameTextbox = document.getElementById("add-lastname");
  const addPasswordTextbox = document.getElementById("add-password");
  const addEmailTextbox = document.getElementById("add-email");
  const addBirthdateTextbox = document.getElementById("add-birthdate");
  const addRoleTextbox = document.getElementById("add-role");

  const item = {
    UserName: addUsernameTextbox.value.trim(),
    First_name: addFirstnameTextbox.value.trim(),
    Last_name: addLastnameTextbox.value.trim(),
    PasswordHash: addPasswordTextbox.value.trim(),
    Email: addEmailTextbox.value.trim(),
    Birth_date: addBirthdateTextbox.value,
    Role: addRoleTextbox.value.trim(),
  };

  fetch(uri, {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify(item),
  })
    .then((response) => response.json())
    .then(() => {
      addUsernameTextbox.value = "";
      addFirstnameTextbox.value = "";
      addLastnameTextbox.value = "";
      addPasswordTextbox.value = "";
      addEmailTextbox.value = "";
      addBirthdateTextbox.value = "";
      addRoleTextbox.value = "";
    })
    .catch((error) => console.error("Unable to add item.", error));
}

/*function closeInput() {
  document.getElementById("editForm").style.display = "none";
}

function _displayCount(itemCount) {
  const name = itemCount === 1 ? "to-do" : "to-dos";

  document.getElementById("counter").innerText = `${itemCount} ${name}`;
}*/

function _displayUsers(data) {
  const tBody = document.getElementById("todos");
  tBody.innerHTML = "";

  _displayCount(data.length);

  const button = document.createElement("button");

  data.forEach((item) => {
    let isCompleteCheckbox = document.createElement("input");
    isCompleteCheckbox.type = "checkbox";
    isCompleteCheckbox.disabled = true;
    isCompleteCheckbox.checked = item.isComplete;

    let editButton = button.cloneNode(false);
    editButton.innerText = "Edit";
    editButton.setAttribute("onclick", `displayEditForm(${item.id})`);

    let deleteButton = button.cloneNode(false);
    deleteButton.innerText = "Delete";
    deleteButton.setAttribute("onclick", `deleteItem(${item.id})`);

    let tr = tBody.insertRow();

    let td1 = tr.insertCell(0);
    td1.appendChild(isCompleteCheckbox);

    let td2 = tr.insertCell(1);
    let textNode = document.createTextNode(item.name);
    td2.appendChild(textNode);

    let td3 = tr.insertCell(2);
    td3.appendChild(editButton);

    let td4 = tr.insertCell(3);
    td4.appendChild(deleteButton);
  });

  todos = data;
}
