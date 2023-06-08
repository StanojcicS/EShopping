import { Link } from "react-router-dom"

export const OldOrdersButton = () => {
    return(
        <Link className="link-text" to='/old-orders'>
            <button className="dashboard-button">Old orders</button>
        </Link>
    )
}