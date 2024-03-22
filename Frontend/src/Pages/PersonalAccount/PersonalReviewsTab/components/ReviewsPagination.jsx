import {createTheme, Pagination, ThemeProvider} from "@mui/material";

export const ReviewsPagination = ({pageCount, currentPage, setCurrentPage}) => {
  const theme = createTheme({
    palette: {
      netflix: {
        main: '#E50914',
        light: '#F51924',
        dark: '#C50004',
        contrastText: '#FFFFFF',
      },
    },
    components: {
      MuiPagination: {
        styleOverrides: {
          root: {
            button: {
              color: "#fff"
            }
          }
        }
      }
    }
  });

  const paginationStyle = {
    marginLeft: "auto",
    marginRight: "auto",
    width: "fit-content",
    backgroundColor: "black",
    borderRadius: "1.2em"
  };

  function handlePageChange(event, page) {
    setCurrentPage(page);
  }

  return (
    <ThemeProvider theme={theme}>
      <Pagination count={pageCount} color={"netflix"} size={"large"}
                  page={currentPage} onChange={handlePageChange}
                  style={paginationStyle}/>
    </ThemeProvider>
  );
}