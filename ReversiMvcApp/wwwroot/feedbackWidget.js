class FeedbackWidget{
    constructor(elementId) {
        this._elementId = elementId;
    }

    get elementId(){
        return this._elementId;
    }

    show(message, type){
        let x = document.getElementById(this.elementId);
        this.log({message: message, type: type});
        x.style.display = "block";
        x.innerText = message;
        switch(type) {
            case "success": {
                x.classList.remove("error");
                x.classList.add("success");
                break;
            }
            case "error": {
                x.classList.remove("success");
                x.classList.add("error");
                break;
            }
        }

    }

    hide(){
        let x = document.getElementById(this.elementId);
        x.style.display = "none";
    }

    log(message){
        let json = localStorage.getItem('feedback_widget');
        let array = [];
        if(json != null){
                array = JSON.parse(json);
        }
        array = array
            //Get a subset of the array at index (length - 9) or 0, whatever is higher, till the end.
            //Aka the last 9 messages
            .slice(Math.max(array.length - 9, 0))
            //Then add the latest message to that array
            .concat([message]);

        localStorage.setItem('feedback_widget', JSON.stringify(array));
    }

    removeLog(){
        localStorage.removeItem('feedback_widget');
    }
}