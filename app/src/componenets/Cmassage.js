import * as React from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import "react-awesome-button/dist/styles.css";
import text from "../elements/Etext";
import "../css/massage.css";

export default function Cmassage() {
  const [open, setOpen] = React.useState(true);
  const handleClose = () => {
    setOpen(false);
    localStorage.setItem("open", "false");
  };

  return (
    <div>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box class="box">
          <Typography
            className="txt"
            id="modal-modal-title"
            variant="h6"
            component="h2"
          >
            {text.welcome}
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            {text.welcome_p}
          </Typography>
          <dev className="message">
            <button type="submit" class="purple-white" onClick={handleClose}>
              {text.button}
            </button>
          </dev>
        </Box>
      </Modal>
    </div>
  );
}
