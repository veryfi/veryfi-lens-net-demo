#!/usr/bin/env python3
"""Validate the standalone veryfi-lens-maui-integrate skill.

Stdlib only. Checks required files, frontmatter, line count, relative links,
banned sibling-skill links, and source-backed phrases. Does not call a Lens API.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent

REQUIRED = [
    "SKILL.md",
    "install.md",
    "syntax.md",
    "bridges.md",
    "legacy.md",
    "recipes.md",
    "settings-groups.md",
    "settings-catalog.md",
    "callbacks.md",
    "conflicts.md",
    "unbridged.md",
    "any-documents.md",
    "clarifications.md",
    "troubleshooting.md",
    "suggestions.md",
    "checklist.md",
    "sources.md",
    "templates/full.md",
    "templates/receipts.md",
    "templates/long-receipts.md",
    "templates/checks.md",
    "templates/credit-cards-ui.md",
    "templates/business-cards.md",
    "templates/barcodes.md",
    "templates/ocr-code-strips.md",
    "templates/ocr-code-caps.md",
    "templates/w2.md",
    "templates/w9.md",
]

REQUIRED_PHRASES = [
    "VeryfiLens.VeryfiLensBinding",
    "VeryfiLens.Maui",
    "UploadImages",
    "isSameTransaction",
    "Unconfirmed",
    "Headless",
    "Lite",
    "Remote Models",
    "retrieveCheckTiffIsOn",
    "credit card",
    "fraudDetectionIsOn",
    "documentFieldsToDetect",
    "backupDocsToGallery",
    "backupDocsToiCloud",
    "https://docs.veryfi.com/lens/mobile/settings/sdk_binary_sizes/",
    "VeryfiLensiOS",
    "quick",
    "guided",
    "showInfoButton",
    "completion(true)",
    "ef01d6b2fe32fcb2db0a10c6b198d8f1597e868a",
    "2.7.0.3",
    "2.0.60",
]

# This pack must stay publishable without the other integration skills.
BANNED_LINK_PARTS = [
    "veryfi-lens-ios-integrate",
    "veryfi-lens-android-integrate",
    "veryfi-lens-plugins-integrate",
    "veryfi-lens-integrate/",
]

LINK_RE = re.compile(r"\[[^\]]+\]\(([^)]+)\)")
FRONTMATTER_RE = re.compile(r"^---\n(.*?)\n---", re.DOTALL)


def fail(errors: list[str], message: str) -> None:
    errors.append(message)


def check_frontmatter(errors: list[str]) -> None:
    text = (ROOT / "SKILL.md").read_text(encoding="utf-8")
    match = FRONTMATTER_RE.match(text)
    if not match:
        fail(errors, "SKILL.md is missing YAML frontmatter")
        return
    body = match.group(1)
    name = re.search(r"^name:\s*(\S+)\s*$", body, re.M)
    desc = re.search(r"^description:\s*>-\s*\n((?:  .+\n)+)", body, re.M)
    if not name or name.group(1) != "veryfi-lens-maui-integrate":
        fail(errors, "SKILL.md name must be veryfi-lens-maui-integrate")
    if not desc:
        fail(errors, "SKILL.md description must use a folded block")
    else:
        description = " ".join(line.strip() for line in desc.group(1).splitlines())
        if not description:
            fail(errors, "SKILL.md description is empty")
        if len(description) > 1024:
            fail(errors, "SKILL.md description exceeds 1024 characters")
        if not description[:1].isupper():
            fail(errors, "SKILL.md description should be third-person and start with a verb or noun")
    line_count = text.count("\n") + (0 if text.endswith("\n") else 1)
    if line_count > 500:
        fail(errors, f"SKILL.md is {line_count} lines; keep it at or under 500")


def check_links(errors: list[str]) -> None:
    for name in REQUIRED:
        text = (ROOT / name).read_text(encoding="utf-8")
        for banned in BANNED_LINK_PARTS:
            if banned in text:
                fail(errors, f"{name} depends on sibling skill path: {banned}")
        for raw in LINK_RE.findall(text):
            target = raw.strip()
            if target.startswith(("http://", "https://", "mailto:")):
                continue
            path_part, _, _anchor = target.partition("#")
            if not path_part:
                continue
            resolved = (ROOT / name).parent / path_part
            if not resolved.exists():
                fail(errors, f"{name} links to missing file: {target}")


def check_phrases(errors: list[str]) -> None:
    blob = "\n".join((ROOT / name).read_text(encoding="utf-8") for name in REQUIRED)
    for phrase in REQUIRED_PHRASES:
        if phrase not in blob:
            fail(errors, f"missing required phrase: {phrase}")
    bridges = (ROOT / "bridges.md").read_text(encoding="utf-8")
    if "ignores" not in bridges.lower() and "not a parameter" not in bridges:
        fail(errors, "bridges.md must state that iOS UploadImages drops isSameTransaction")
    if "completion(true)" not in bridges and "Invoke(true)" not in bridges:
        fail(errors, "bridges.md must state Android configure completion is always true")
    if "drops" not in bridges and "not forwarded" not in bridges:
        fail(errors, "bridges.md must state Android retry drops metadata and tag")
    skill = (ROOT / "SKILL.md").read_text(encoding="utf-8")
    if "unsupported" not in skill.lower():
        fail(errors, "SKILL.md must mark Headless and Lite unsupported")


def main() -> int:
    errors: list[str] = []
    for name in REQUIRED:
        if not (ROOT / name).is_file():
            fail(errors, f"missing {name}")
    if not errors:
        check_frontmatter(errors)
        check_links(errors)
        check_phrases(errors)
    if errors:
        print("FAIL")
        for item in errors:
            print(f"- {item}")
        return 1
    print("OK")
    print(f"files: {len(REQUIRED)}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
