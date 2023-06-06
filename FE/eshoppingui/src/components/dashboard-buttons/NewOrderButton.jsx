import { Link } from "react-router-dom"

export const NewOrderButton = () => {
    return(
        <Link className="link-text" to='/new-order'>
            <button className="dashboard-button">New order</button>
        </Link>
    )
}