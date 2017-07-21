function MouseEnter(row, bgColor, textColor) {
    if (row.getAttribute("Selected") == "true") return;
    row.style.backgroundColor = bgColor;
    row.style.color = textColor;
}

function MouseLeave(row) {
    if (row.getAttribute("Selected") == "true") return;
    row.style.backgroundColor = row.getAttribute("OriginalColor");
    row.style.color = row.getAttribute("OriginalTextColor");
}

function MouseDown(row, bgColor, textColor) {
    if (row.getAttribute("Selected") == "true") {
        row.style.backgroundColor = row.getAttribute("OriginalColor");
        row.style.color = row.getAttribute("OriginalTextColor");
        row.setAttribute("Selected", "false");
    }
    else {
        row.style.backgroundColor = bgColor;
        row.style.color = textColor
        row.setAttribute("Selected", "true");
    }
}

