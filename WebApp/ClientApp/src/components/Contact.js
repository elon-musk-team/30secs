import {Component} from "react";

export class Contact extends Component {
    static displayName = Contact.name;
    
    render() {
        let selectedClass = this.props.isSelected
            ? " selected"
            : "";

        return (
            <div className={"user" + selectedClass} onClick={this.props.onClick}>
                <div className="avatar">
                    <img className="avatar-circle" src={this.props.linkToAvatar} alt="User name"/>
                    <div className="status off"/>
                </div>
                <div className="ct-name">{this.props.screenName}</div>
                <div className="ct-mood">additional info might go here</div>
            </div>
        )
    }
}
