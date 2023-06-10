import { Link } from "react-router-dom"

export const NewOrdersButton = () => {
    return(
        <Link className="link-text" to="/new-orders">
            <button className="dashboard-button">New orders</button>
        </Link>
    )
}