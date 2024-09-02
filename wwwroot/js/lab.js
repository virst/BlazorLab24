var CellOptions = {
    EXIT_EAST: 0x01, //���� ������ �� ������
    EXIT_NORTH: 0x02, //���� ������ �� �����
    EXIT_WEST: 0x04, //���� ������ �� �����
    EXIT_SOUTH: 0x08, //���� ������ �� ��
    CELL_FRONTIER: 0x20, //���������
    CELL_VISITED: 0x10,  // ���������� 
    NONE: 0x00  // �����
}

let cellWidth = 50;

function AddFlag(e, v) {
    e |= v;
    return e;
}

function RemoveFlag(e, v) {
    e &= ~v;
    return e;
}

function HasFlag(e, v) {
    return (e & v) == v;
}

function AddLine(ctx, x1, y1, x2, y2) {
    ctx.beginPath();
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
}

function FillRectangle(ctx, color, x, y, w, h) {
    ctx.fillStyle = color;
    ctx.fillRect(x, y, w, h); // ������ �������
}

class Rcv {
    constructor(w) {
        this.w = w;
        this.v = 0;
        this.r = 0;
        this.c = 0;
    }

    SetRC(r, c) {
        this.r = r;
        this.c = c;
        this.v = r * this.w + c;
    }

    SerVal(v) {
        this.v = v;
        this.r = Math.floor(v / this.w);
        this.c = v % this.w;
    }
}

window.SetCellWidth = (w) => { cellWidth = w };

window.DrawLab = (canvasId, map, start, fin) => {
    const startColor = "#FFA500";
    const finColor = "#008000"

    //console.log(start);
    //console.log(fin);
    // �������� ������� canvas �� ��� id
    const canvas = document.getElementById(canvasId);
    // ���������, ��� ������� ���������� � ��� ��� canvas
    if (!canvas || !(canvas instanceof HTMLCanvasElement)) {
        console.error('������� canvas � ��������� ID �� ������ ��� �� �������� Canvas ���������.');
        return;
    }

    const ctx = canvas.getContext('2d');
    if (!ctx) {
        console.error('�� ������� �������� �������� ��������� ��� Canvas.');
        return;
    }

    // ������ ������ ��������� ������    
    const lineWidth = 3;
    let h = map.length;
    let w = map[0].length;

    // ������� canvas ����� ������� ���������
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    if (start != null) {
        FillRectangle(ctx, startColor, start.col * cellWidth + lineWidth
            , start.row * cellWidth + lineWidth, cellWidth - lineWidth * 2, cellWidth - lineWidth * 2);
    }
    if (fin != null) {
        FillRectangle(ctx, finColor, fin.col * cellWidth + lineWidth
            , fin.row * cellWidth + lineWidth, cellWidth - lineWidth * 2, cellWidth - lineWidth * 2);
    }

    // ������������� ���� ����� ����� (�� ��������� ������)
    ctx.strokeStyle = "#0000FF";
    // ������������� ������ �����
    ctx.lineWidth = lineWidth;

    for (let i = 0; i < h; i++) {
        for (let j = 0; j < w; j++) {
            if (!HasFlag(map[i][j], CellOptions.EXIT_NORTH)) {
                AddLine(ctx, j * cellWidth, i * cellWidth, (j + 1) * cellWidth, i * cellWidth);
            }
            if (!HasFlag(map[i][j], CellOptions.EXIT_EAST)) {
                AddLine(ctx, (j + 1) * cellWidth, i * cellWidth, (j + 1) * cellWidth, (i + 1) * cellWidth);
            }
            if (!HasFlag(map[i][j], CellOptions.EXIT_SOUTH)) {
                AddLine(ctx, (j + 1) * cellWidth, (i + 1) * cellWidth, j * cellWidth, (i + 1) * cellWidth);
            }
            if (!HasFlag(map[i][j], CellOptions.EXIT_WEST)) {
                AddLine(ctx, j * cellWidth, i * cellWidth, j * cellWidth, (i + 1) * cellWidth);
            }
        }
    }
}

window.DrawWay = (canvasId, dr, st, fin, MaxValue) => {
    //console.log(dr);
    //console.log(st);
    //console.log(fin);
    if (dr == null || st == null || fin == null) return;
    const wayColor = "#FF0000"
    const canvas = document.getElementById(canvasId);
    // ���������, ��� ������� ���������� � ��� ��� canvas
    if (!canvas || !(canvas instanceof HTMLCanvasElement)) {
        console.error('������� canvas � ��������� ID �� ������ ��� �� �������� Canvas ���������.');
        return;
    }

    const ctx = canvas.getContext('2d');
    if (!ctx) {
        console.error('�� ������� �������� �������� ��������� ��� Canvas.');
        return;
    }

    // ������ ������ ��������� ������
    const lineWidth = 3;

    // ������������� ���� ����� ����� (�� ��������� ������)
    ctx.strokeStyle = wayColor;
    // ������������� ������ �����
    ctx.lineWidth = lineWidth;

    const r1 = new Rcv(canvas.width / cellWidth);
    const r2 = new Rcv(canvas.width / cellWidth);
    r1.SetRC(st.row, st.col);
    r2.SetRC(fin.row, fin.col);

    let start = r1.v;
    let wayTo = r2.v;

    console.log(start);
    console.log(wayTo);

    if (wayTo > -1 && start > -1 && dr.dist[wayTo] != MaxValue) {
        let now = wayTo;
        while (now != start) {
            let next = dr.parent[now];
            //Pen p = new Pen(Color.Red, 4);
            r1.SerVal(now);
            r2.SerVal(next);
            console.log(r2);

            AddLine(ctx, r1.c * cellWidth + cellWidth / 2, r1.r * cellWidth + cellWidth / 2,
                r2.c * cellWidth + cellWidth / 2, r2.r * cellWidth + cellWidth / 2);
            now = next;
        }
    }
}