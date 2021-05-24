import {Component} from "react";

export class Contact extends Component {
    static displayName = Contact.name;

    constructor(props) {
        super(props);
    }
    
    isSelectedToClass(){
        return this.props.isSelected 
            ? " selected" 
            : "";
    }
    
    render() {
        return (
            <div className={"user" + this.isSelectedToClass()} onClick={this.props.onClick}>
                <div className="avatar">
                    <img className="avatar-circle" src={this.props.avatarLink} alt="User name"/>
                    <div className="status off"/>
                </div>
                <div className="ct-name">{this.props.screenName}</div>
                <div className="ct-mood">additional info might go here</div>
            </div>
        )
    }
}
