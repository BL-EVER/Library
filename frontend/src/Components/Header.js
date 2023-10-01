import {useOidc} from "@axa-fr/react-oidc";
import {AppBar, Box, Button, IconButton, Toolbar, Typography} from "@mui/material";
import {Link} from "react-router-dom";
import AccountCircleIcon from '@mui/icons-material/AccountCircle';



const Header = ({children}) => {
    const { login, logout, renewTokens, isAuthenticated } = useOidc();
    return (
        <div>
            <Box sx={{ flexGrow: 1 }}>
                <AppBar position="static">
                    <Toolbar>
                        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                            <Link to='/' style={{textDecoration: 'none', color: 'white'}}>
                                Movie application
                            </Link>
                        </Typography>

                        {!isAuthenticated && (
                            <Button
                                color="inherit"
                                onClick={() => login("/")}
                            >
                                Login
                            </Button>
                        )}
                        {isAuthenticated && (
                            <>
                                <Link to='/' style={{textDecoration: 'none'}}>
                                    <IconButton
                                        size="large"
                                        edge="start"
                                        color="inherit"
                                        aria-label="menu"
                                        sx={{ mr: 2 }}
                                        style={{color: 'white'}}
                                    >
                                        <AccountCircleIcon />
                                    </IconButton>
                                </Link>
                                <Button
                                    color="inherit"
                                    onClick={() => logout()}
                                >
                                    logout
                                </Button>
                            </>
                        )}
                    </Toolbar>
                </AppBar>
            </Box>

            {children}
        </div>
    );
};

export default Header;