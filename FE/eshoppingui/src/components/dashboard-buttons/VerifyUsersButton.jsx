import { Link } from "react-router-dom"

export const VerifyUsersButton = () => {
    return(
        <Link className="link-text" to='/verify-users'>
            <button className="dashboard-button">Verify users</button>
        </Link>)
}