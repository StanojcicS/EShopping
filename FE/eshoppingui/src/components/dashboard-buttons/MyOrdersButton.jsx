import { Link } from "react-router-dom"

export const MyOrdersButton = () => {
    return(
        <Link className="link-text" to='/my-orders'>
            <button className="dashboard-button">My orders</button>
        </Link>
    )
}