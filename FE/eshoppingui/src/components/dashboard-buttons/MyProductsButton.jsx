import { Link } from "react-router-dom"

export const MyProductsButton = () => {
    return(
        <Link className="link-text" to='/my-products'>
            <button className="dashboard-button">My products</button>
        </Link>
    )
}