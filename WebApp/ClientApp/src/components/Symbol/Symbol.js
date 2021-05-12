import {Component} from "react";
import './style.css';

export class Symbol extends Component {
    static displayName = Symbol.name;

    render() {
        let borderClass = "symbol-border " + this.props.borderColorClass;
        return (
            <div className="symbol-wrapper">
                <div className={borderClass}>
                    {this.props.content}
                </div>
            </div>
        )
    }
}