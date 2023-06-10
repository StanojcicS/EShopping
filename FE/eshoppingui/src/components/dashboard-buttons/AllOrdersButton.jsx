import { Link } from "react-router-dom"

export const AllOrdersButton = () => {
    return(
        <Link className="link-text" to="/all-orders">
        <button className="dashboard-button">All orders</button>
        </Link>
    )
}