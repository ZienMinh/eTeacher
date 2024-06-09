const uri = "api/Auth/register";
let users = [];

function getUsers() {
  fetch(uri)
    .then((response) => response.json())
    .then((data) => _displayUsers(data))
    .catch((error) => console.error("Unable to get items.", error));
}

function addUsers() {
  const addFirstnameTextbox = document.getElementById("add-firstname");
  const addLastnameTextbox = document.getElementById("add-lastname");
  const addPasswordTextbox = document.getElementById("add-password");
  const addEmailTextbox = document.getElementById("add-email");
  const addAddressTextbox = document.getElementById("add-address");
  const addPhoneNumberTextbox = document.getElementById("add-phonenumber");
  const addBirthdateTextbox = document.getElementById("add-birthdate");
  const addLinkContactTextbox = document.getElementById("add-linkcontact");
  const addImageTextbox = document.getElementById("add-image");
  const addRatingTextbox = document.getElementById("add-rating");
  const addRoleTextbox = document.getElementById("add-role");

  const item = {
    isComplete: false,
    name: addNameTextbox.value.trim(),
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
      getItems();
      addNameTextbox.value = "";
    })
    .catch((error) => console.error("Unable to add item.", error));
}

function closeInput() {
  document.getElementById("editForm").style.display = "none";
}

function _displayCount(itemCount) {
  const name = itemCount === 1 ? "to-do" : "to-dos";

  document.getElementById("counter").innerText = `${itemCount} ${name}`;
}

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
