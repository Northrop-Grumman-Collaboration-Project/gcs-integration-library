from dataclasses import dataclass
from Types.Geolocation import Coordinate, Polygon
from enum import Enum, auto
from typing import Any


@dataclass(repr=False)
class Commands:
    isManual: bool
    target: Coordinate
    searchArea: Polygon

    def to_dict(self) -> Dict:
        obj = {
            'isManual': self.isManual,
            'target': self.target.to_dict(),
            'searchArea': self.searchArea.to_dict(),
        }
        return obj
