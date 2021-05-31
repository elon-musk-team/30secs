import {Component} from "react";
import './style.css';

export class Symbol extends Component {
    static displayName = Symbol.name;

    overHandler(){
        return true;
    }
    render() {
        let borderClass = "symbol-border " + this.props.borderColorClass;
        if (this.props.isFirst)
            borderClass += " first-symbol";
        else if (this.props.isLast)
            borderClass += " last-symbol";
        
        return (
            <div className="symbol-wrapper">
                <div className={borderClass} title={this.props.screenName}>
                    {this.props.content}
                </div>
            </div>
        )
    }
}