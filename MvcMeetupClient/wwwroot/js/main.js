let eventList = {
    page: 1,
    targetFunc: () => GetEvents()
}

window.addEventListener('load', function () {
    document.querySelector("#pageNumber").innerHTML = eventList.page;
    eventList.targetFunc();
});

var addModal = {
    MeetupName : document.querySelector(".add input[name='MeetupName']"),
    Theme : document.querySelector(".add input[name='Theme']"),
    Description : document.querySelector(".add textarea[name='Description']"),
    Schedule : function () {
        let model = [];
        let values = document.querySelectorAll(".add .planItemValue");
        for(let value of values) {
            model.push(value.innerText);
        }

        return model;
    },
    Speeker : document.querySelector(".add input[name='Speeker']"),
    Time : document.querySelector(".add input[name='Time']"),
    Location : document.querySelector(".add input[name='Location']"),
    ViewSchedule: function(jsonPreset) {
        let holder = document.querySelector(".add #planHolder");
        let preset = JSON.parse(jsonPreset);

        for(let v of preset) {
            let amount = document.querySelectorAll(".add .planItem").length;
            let tag = "     <li class=\"p-1 px-2 list-group-item border-0 d-flex align-items-center justify-content-between planItem planItem "+ amount +"\">\n" +
                "                       <div class='btn planItemValue'>\n" +
                v +
                "                       </div>\n" +
                "                       <button class=\"btn bg-transparent\" onclick=\"addModal.RemoveScheduleItem(" + amount + ")\">\n" +
                "                           <span aria-hidden=\"true\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></span>\n" +
                "                       </button>\n"
            "                   </li>";

            holder.innerHTML += tag;
        }
    },
    AddScheduleItem : function () {
        let value = document.querySelector(".add #planInput").value;
        let holder = document.querySelector(".add #planHolder");
        let amount = document.querySelectorAll(".add .planItem").length;

        let tag = "     <li class=\"p-1 px-2 list-group-item border-0 d-flex align-items-center justify-content-between planItem planItem" + amount + "\">\n" +
            "                       <span class='planItemValue'>\n" +
            value +
            "                       </span>\n" +
            "                       <button class=\"btn bg-transparent\" onclick=\"addModal.RemoveScheduleItem(" + amount + ")\">\n" +
            "                           <span aria-hidden=\"true\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></span>\n" +
            "                       </button>\n" +
            "                   </li>";

        holder.innerHTML += tag;

        document.querySelector(".add #planInput").value = "";
    },
    RemoveScheduleItem : function (itemId) {
        let a = document.querySelector(".add .planItem" + itemId);
        a.remove();
    },
    ClearModal: function() {
        this.MeetupName.value = "";
        this.Theme.value = "";
        this.Description.value = "";
        this.Speeker.value = "";
        this.Time.value = "";
        this.Location.value = "";
        document.querySelector(".add #planHolder").innerHTML = "";
    }
}

let editModal = {
    IsUpdating : false,
    EventId : null,
    MeetupName : document.querySelector(".update input[name='MeetupName']"),
    Theme : document.querySelector(".update input[name='Theme']"),
    Description: document.querySelector(".update textarea[name='Description']"),
    Schedule : function () {
        let model = [];
        let values = document.querySelectorAll(".update .planItemValue");
        for(let value of values) {
            model.push(value.innerText);
        }

        return model;
    },
    Speeker : document.querySelector(".update input[name='Speeker']"),
    Time : document.querySelector(".update input[name='Time']"),
    Location : document.querySelector(".update input[name='Location']"),
    ViewSchedule: function(jsonPreset) {
        let holder = document.querySelector(".update #planHolder");
        let preset = JSON.parse(jsonPreset);
        if(jsonPreset == null) return;

        for(let v of preset) {
            let amount = document.querySelectorAll(".update .planItem").length;
            let tag = "     <li class=\"p-1 px-2 list-group-item border-0 d-flex align-items-center justify-content-between planItem planItem"+ amount +"\">\n" +
                "                       <div class='btn planItemValue'>\n" +
                v +
                "                       </div>\n" +
                "                       <button class=\"btn bg-transparent\" onclick=\"editModal.RemoveScheduleItem(" + amount + ")\">\n" +
                "                           <span aria-hidden=\"true\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></span>\n" +
                "                       </button>\n"
                "                   </li>";

            holder.innerHTML += tag;
        }
    },
    AddScheduleItem : function () {
        let value = document.querySelector(".update #planInput").value;
        let holder = document.querySelector(".update #planHolder");
        let amount = document.querySelectorAll(".update .planItem").length;
    
        let tag = "     <li class=\"p-1 px-2 list-group-item border-0 d-flex align-items-center justify-content-between planItem planItem" + amount + "\">\n" +
            "                       <span class='planItemValue'>\n" +
            value +
            "                       </span>\n" +
            "                       <button class=\"btn bg-transparent\" onclick=\"editModal.RemoveScheduleItem(" + amount + ")\">\n" +
            "                           <span aria-hidden=\"true\"><i class=\"fa fa-trash\" aria-hidden=\"true\"></i></span>\n" +
            "                       </button>\n" +
            "                   </li>";
    
        holder.innerHTML += tag;
    
        document.querySelector(".update #planInput").value = "";
    },
    RemoveScheduleItem : function (itemId) {
        let a = document.querySelector(".update .planItem" + itemId);
        a.remove();
    },
    ClearModal: function() {
        this.MeetupName.value = "";
        this.Theme.value = "";
        this.Description.value = "";
        this.Speeker.value = "";
        this.Time.value = "";
        this.Location.value = "";
        document.querySelector(".update #planHolder").innerHTML = "";
    }
}

let viewModal = {
    Id : document.querySelector(".view div[name='ID']"),
    MeetupName : document.querySelector(".view .modal-title[name='MeetupName']"),
    Theme : document.querySelector(".view div[name='Theme']"),
    Description: document.querySelector(".view div[name='Description']"),
    Speeker : document.querySelector(".view div[name='Speeker']"),
    Time : document.querySelector(".view div[name='Time']"),
    Location : document.querySelector(".view div[name='Location']"),
    ViewSchedule: function(jsonPreset) {
        let holder = document.querySelector(".view #planHolder");
        let preset = JSON.parse(jsonPreset);
        if(jsonPreset == null) return;

        for(let v of preset) {
            let tag = "     <li class=\"p-1 px-2 list-group-item border-0 d-flex align-items-center justify-content-between planItem\">\n" +
                "                       <div class='btn planItemValue'>\n" +
                v +
                "                       </div>\n" +
                "                   </li>";

            holder.innerHTML += tag;
        }
    },
    ClearModal: function() {
        this.Id.innerHTML = "";
        this.MeetupName.innerHTML = "";
        this.Theme.innerHTML = "";
        this.Description.innerHTML = "";
        this.Speeker.innerHTML = "";
        this.Time.innerHTML = "";
        this.Location.innerHTML = "";
        document.querySelector(".view #planHolder").innerHTML = "";
    }
}

// API requests

function SetTargetToEvents() {
    eventList.page = 1;
    eventList.targetFunc = () => GetEvents();
    document.querySelector("#pageNumber").innerHTML = eventList.page;
    eventList.targetFunc();
}


function GetEvents() {
    fetch("/RequestEvents?page=" + eventList.page + "&amount=2", {
        method: "get"
    }).then(r => r.json()).then(d => {
        if(d.success == true) {
            CreateEventList(d.info, false);
        }
    });
}

function SetTargetToMyEvents() {
    eventList.page = 1;
    eventList.targetFunc = () => GetMyEvents();
    document.querySelector("#pageNumber").innerHTML = eventList.page;
    eventList.targetFunc();
}

function GetMyEvents() {
    fetch("/RequestMyEvents?page=" + eventList.page + "&amount=2", {
        method: "get",
    }).then(r => r.json()).then(d => {
        if(d.success == true) {
            CreateEventList(d.info, true);
        }
    });
}

function AddEvent() {
    let model = {
        MeetupName: addModal.MeetupName.value,
        Theme: addModal.Theme.value,
        Description: addModal.Description.value,
        Schedule: JSON.stringify(addModal.Schedule()),
        SpeekerName: addModal.Speeker.value,
        Time: addModal.Time.value,
        location: addModal.Location.value
    }
    
    console.log(model);

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
    fetch("/RequestEvent?id=" + eventId, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }).then(response => response.json()).then(data => {
        if (data.success == true) {
            let ev = data.info;
            editModal.ClearModal();
            editModal.MeetupName.value = ev.meetupName;
            editModal.Theme.value = ev.theme;
            editModal.Description.value = ev.description;
            editModal.Speeker.value = ev.speekerName;
            editModal.Time.valueAsDate = new Date(ev.time);
            editModal.Location.value = ev.location;
            editModal.ViewSchedule(ev.schedule);
            
            editModal.IsUpdating = true;
            editModal.EventId = eventId;
        }
    });
}

function UpdateEvent() {
    if (editModal.IsUpdating) {
        let model = {
            Id : editModal.EventId,
            MeetupName : editModal.MeetupName.value,
            Theme : editModal.Theme.value,
            Description : editModal.Description.value,
            Schedule : JSON.stringify(editModal.Schedule()),
            SpeekerName : editModal.Speeker.value,
            Time : editModal.Time.value,
            Location : editModal.Location.value
        }

        fetch("/UpdateEvent", {
            method: "POST",
            body: JSON.stringify(model),
            headers: {
                "Content-Type": "application/json"
            }
        }).then(response => response.json()).then(data => {
            if(data.success == true) {
                editModal.IsUpdating = false;
                editModal.EventId = false;
                
                window.location.reload();
            } 
        });
    }
}

function DissmissUpdating() {
    editModal.IsUpdating = false;
    editModal.EventId = false;
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

function ViewEvent(eventId) {
    fetch("/RequestEvent?id=" + eventId, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    }).then(response => response.json()).then(data => {
        if (data.success == true) {
            let ev = data.info;
            
            viewModal.ClearModal();
            viewModal.Id.innerText = ev.id;
            viewModal.MeetupName.innerHTML = ev.meetupName;
            viewModal.Theme.innerHTML = ev.theme;
            viewModal.Description.innerHTML = ev.description;
            viewModal.Speeker.innerHTML = ev.speekerName;
            viewModal.Time.innerHTML = ev.time;
            viewModal.Location.innerHTML = ev.location;
            viewModal.ViewSchedule(ev.schedule);
        }
    });
}

function Search() {
    let value = document.querySelector("#search").value;
    
    if(value.slice(0,3) == "id:") {
        value = value.slice(3);

        fetch("/RequestEvent?id=" + value, {
            method: "get"
        }).then(r => r.json()).then(d => {
            if(d.success == true) {
                CreateEventList([d.info], false);
            }
        });
        
    } 
    else {
        SetTargetToEvents();
    }
}

function ListNext() {
    eventList.page += 1;
    document.querySelector("#pageNumber").innerText = eventList.page;
    eventList.targetFunc();
}

function ListPrev() {
    if(eventList.page == 1) return;
    eventList.page --;
    document.querySelector("#pageNumber").innerText = eventList.page;
    eventList.targetFunc();
}

function ShowEventList(isShown) {
    let row = document.querySelector("#list-content");
    row.style.display = (isShown) ? "block" : "none";
}

function CreateEventList(events, isManager) {
    ShowEventList(true);

    let tbody = document.querySelector("#table-event-container");
    tbody.innerHTML = "";

    let inner = "";

    for (let event of events) {
        inner += "<tr>" +
            "<th scope=\"row\" class=\"align-middle\">"+ event.id +"</th>" +
            "<td class=\"align-middle\">"+ event.meetupName +"</td>" +
            "<td class=\"align-middle\">"+ event.theme +"</td>" +
            "<td class=\"align-middle\">"+ event.location +"</td>" +
            "<td class=\"align-middle\">"+ event.time +"</td>" +
            "<td class=\"align-middle\">" +
            "   <div class=\"dropright d-flex justify-content-end\">" +
            "       <button class=\"btn btn-primary mr-1\" data-toggle=\"modal\" data-target=\"#eventModalView\" onclick=\"ViewEvent(" + event.id + ")\">" +
            "           <i class=\"fa fa-eye\" aria-hidden=\"true\"></i>" +
            "       </button>";

        if(isManager) {
            inner +=
                "       <button class=\"btn btn-primary dropdown-toggle\" type=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">" +
                "       </button>\n    " +
                "       <div class=\"dropdown-menu\" aria-labelledby=\"triggerId\">" +
                "           <a class=\"dropdown-item\" href=\"javascript:void(0)\" data-toggle=\"modal\" data-target=\"#eventModalEdit\" onclick=\"EditEvent(" + event.id + ")\">Edit</a>" +
                "           <a class=\"dropdown-item disabled\" href=\"javascript:void(0)\" onclick=\"DeleteEvent(" + event.id + ")\">Remove</a>" +
                "       </div>" +
                "   </div>\<" +
                "/td>" +
                "</tr>";
        }
        else {
            inner += " </div>" +
                "   </td>" +
                "</tr>";
        }
    }

    tbody.innerHTML = inner;
}
