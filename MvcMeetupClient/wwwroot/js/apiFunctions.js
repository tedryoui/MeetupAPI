let addFormInput = {
    MeetupName : document.querySelector(".add input[name='MeetupName']"),
    Theme : document.querySelector(".add input[name='Theme']"),
    Description : document.querySelector(".add input[name='Description']"),
    Schedule : null,
    Speeker : document.querySelector(".add input[name='Speeker']"),
    Time : document.querySelector(".add input[name='Time']"),
    Location : document.querySelector(".add input[name='Location']")
}

let updateFormInput = {
    IsUpdating : false,
    EventId : null,
    Form : document.querySelector(".Update"),
    MeetupName : document.querySelector(".Update input[name='MeetupName']"),
    Theme : document.querySelector(".Update input[name='Theme']"),
    Description: document.querySelector(".Update input[name='Description']"),
    Schedule : null,
    Speeker : document.querySelector(".Update input[name='Speeker']"),
    Time : document.querySelector(".Update input[name='Time']"),
    Location : document.querySelector(".Update input[name='Location']")
}

// API requests

function GetEvent() {
    let id = document.querySelector(".idSearch input").value;
    
    fetch("/ReceiveEvent?id=" + id, {
        method: "get"
    }).then(r => r.json()).then(d => {
        if(d.success == true) {
            console.log(d.info);
            
            DisplayEvent(d.info);
        }
    });
}

function GetEvents() {
    fetch("/RequestEvents", {
        method: "get"
    }).then(r => r.json()).then(d => {
        if(d.success == true) {
            DisplayEvents(d.info);
        }
    });
}

function GetMyEvents() {
    fetch("/RequestMyEvents", {
        method: "get",
    }).then(r => r.json()).then(d => {
        if(d.success == true) {
            DisplayMyEvents(d.info);
        }
    });
}

function AddEvent() {
    let model = {
        MeetupName: addFormInput.MeetupName.value,
        Theme: addFormInput.Theme.value,
        Description: addFormInput.Description.value,
        Schedule: null,
        Speeker: addFormInput.Speeker.value,
        Time: addFormInput.Time.value,
        location: addFormInput.Location.value
    }

    fetch("/RequestAdd", {
        method: "POST",
        body: JSON.stringify(model),
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            if(data.success == true) 
                window.location.reload()
        });
}

function EditEvent(eventId) {
    updateFormInput.Form.style.display = "block";

    fetch("/ReceiveEvent?id=" + eventId, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }).then(response => response.json()).then(data => {
        if (data.success == true) {
            let ev = data.info;

            updateFormInput.MeetupName.value = ev.meetupName;
            updateFormInput.Theme.value = ev.theme;
            updateFormInput.Description.value = ev.description;
            updateFormInput.Speeker.value = ev.speeker;
            updateFormInput.Time.valueAsDate = new Date(ev.time);
            updateFormInput.Location.value = ev.location;

            updateFormInput.IsUpdating = true;
            updateFormInput.EventId = eventId;
        }
    });
}

function UpdateEvent() {
    if (updateFormInput.IsUpdating) {

        let model = {
            Id : updateFormInput.EventId,
            MeetupName : updateFormInput.MeetupName.value,
            Theme : updateFormInput.Theme.value,
            Description : updateFormInput.Description.value,
            Speeker : updateFormInput.Speeker.value,
            Time : updateFormInput.Time.value,
            Location : updateFormInput.Location.value
        }

        fetch("/UpdateEvent", {
            method: "POST",
            body: JSON.stringify(model),
            headers: {
                "Content-Type": "application/json"
            }
        }).then(response => response.json()).then(data => {
            if(data.success == true) {
                updateFormInput.Form.style.display = "none";
                updateFormInput.IsUpdating = false;
                updateFormInput.EventId = false;
            } 
        });
    }
}

function DeleteEvent(eventId) {
    fetch("/RequestDelete?id=" + eventId, {
        method: "DELETE",
    }).then(r => r.json()).then(d => {
        if(d.success == true) {
            window.location.reload();
        }
    });
}

function GetUserInfo() {
    
}

// Building tag functions

function DisplayEvent(data) {
    let container = document.querySelector("#searchContainer");
    container.innerHTML = "";

    let elementContainer = BuildEventElementContainer();

    elementContainer.append(BuildEventTextContainer(data.meetupName));
    elementContainer.append(BuildEventTextContainer(data.theme));
    elementContainer.append(BuildEventTextContainer(data.time));

    container.append(elementContainer);
}

function DisplayEvents(data) {
    let container = document.querySelector("#searchContainer");
    container.innerHTML = "";

    for (let event of data) {
        let elementContainer = BuildEventElementContainer();

        elementContainer.append(BuildEventTextContainer(event.meetupName));
        elementContainer.append(BuildEventTextContainer(event.theme));
        elementContainer.append(BuildEventTextContainer(event.time));

        container.append(elementContainer);
    }
}

function DisplayMyEvents(data) {
    let container = document.querySelector("#searchContainer");
    container.innerHTML = "";

    for (let event of data) {
        let elementContainer = BuildEventElementContainer();

        elementContainer.append(BuildEventTextContainer(event.meetupName));
        elementContainer.append(BuildEventTextContainer(event.theme));
        elementContainer.append(BuildEventTextContainer(event.time));
        elementContainer.append(BuildOrgonizerTools(event.id));

        container.append(elementContainer);
    }
}

function BuildEventElementContainer() {
    let div = document.createElement("div");
    div.classList.add("event-element");

    return div;
}

function BuildEventTextContainer(text) {
    let textTag = document.createElement("span");
    textTag.classList.add("event-element-text");
    textTag.innerText = text;

    return textTag;
}

function BuildOrgonizerTools(eventId) {
    let div = document.createElement("div");
    div.classList.add("event-element-tools");

    let removeButton = document.createElement("button");
    removeButton.innerText = "Remove";
    removeButton.classList.add("event-element-tools-remove");
    removeButton.addEventListener("click", () => DeleteEvent(eventId));

    let editButton = document.createElement("button");
    editButton.innerText = "Edit";
    editButton.classList.add("event-element-tools-edit");
    editButton.addEventListener("click", () => EditEvent(eventId));

    div.append(removeButton);
    div.append(editButton);

    return div;
}
