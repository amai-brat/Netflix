import {Navigate, Outlet} from "react-router-dom";
import {authenticationService} from "../../../services/authentication.service.js";

export const ProtectedRoute = ({roles}) => {
  const user = authenticationService.getUser();

  if (!user) {
    return <Navigate to={"/signin"}/>
  }

  if (user && roles.indexOf(user.role) === -1) {
    return <Navigate to={"/"}/>
  }

  return <Outlet/>;
}