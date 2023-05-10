import { Link } from "react-router-dom"

export const MyProfileButton = () => {
    return(
        <Link className="link-text" to='/my-profile'>
            <button className="dashboard-button">My profile</button>
            </Link>
    )
}