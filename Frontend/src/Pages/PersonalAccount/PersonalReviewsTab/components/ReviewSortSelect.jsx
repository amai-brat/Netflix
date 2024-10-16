import {createTheme, MenuItem, Select, ThemeProvider} from "@mui/material";

export const ReviewSortSelect = ({setSortType}) => {
  const theme = createTheme({
    palette: {
      text: {
        primary: '#ffffff',
      },
    },
    components: {
      MuiSelect: {
        styleOverrides: {
          root: {
            color: "white",
            fontSize: "1.2em",
            marginLeft: "0.5em"
          },
          select: {
            color: "white",
          },
          standard: {
            color: "white",
            borderColor: "white"
          },
        }
      },
      MuiMenu: {
        styleOverrides: {
          list: {
            backgroundColor: "#313131"
          }
        }
      },
      MuiMenuItem: {
        styleOverrides: {
          root: {
            "&.Mui-selected": {
              "&:hover": {
                "backgroundColor": "#515151"
              },
            "backgroundColor": "#414141"
            }
          },
          gutters: {
            fontSize: "1.2em",
            "&:hover": {
              "backgroundColor": "#515151"
            },
          }
        }
      }
    }
  });
  
  return (
    <ThemeProvider theme={theme}>
      <div>
        <Select variant={"standard"} defaultValue={"rating"} label={"Sort"} 
                onChange={(event) => setSortType(event.target.value)}>
          <MenuItem value={"date-updated"}>дате обновления</MenuItem>
          <MenuItem value={"rating"}>оценке</MenuItem>
        </Select>
      </div>
    </ThemeProvider>
  );
}